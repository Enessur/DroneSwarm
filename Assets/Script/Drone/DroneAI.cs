using System;
using System.Security.Cryptography;
using Drone;
using Script;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DroneAI : MonoBehaviour
{
    public Rigidbody _rb;
    public DroneScriptableObject item;
    public DroneDataScriptable data;
    public Collectable _collectable;
    public EnemyBehaviour _enemyTarget;
    public Transform _droneStationTransform;
    public float collectTimer = 1f; // get set yapılmıyor
    [SerializeField] private  int _storageCapacity = 20;

    public float timer { get; set; }= 0;
    public int Stored { get; set; }= 0;
    
    private Vector3 _standartPrediction, _deviatedPrediction;
    // [SerializeField] private float _maxTimePrediction = 5f;
    
    //Prediction
    [SerializeField] private DroneMovementManager _droneMovementManager;
    [SerializeField] private LayerMask whatIsEnemies;
    private Quaternion targetRotation;
    private Vector3 previousPosition;
    private float x, y, z;
    // private float _findStationRange = 1f;
    private DroneStation _motherShipStation;
    private DroneStation _currentStation;
    private StateMachine _stateMachine;

    [FormerlySerializedAs("_isCollecting")] [SerializeField]
    public bool _isStorageFull;

    private void Awake()
    {
        _stateMachine = new StateMachine();

        var chase = new DroneChaseState();
        var follow = new DroneFollowState();
        var collect = new DroneCollectState();
        
        At(follow,chase,HasTarget());
        At(collect,chase, HasTarget());
        At(follow, collect, HasCollectable()); 
        At(chase,follow,HasNoTarget());
        At(collect,follow,HasNoCollectable());
        
        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
        
        _stateMachine.SetState(follow);
        Func<bool> HasTarget()=> () => _enemyTarget != null &&
                                       Vector3.Distance(_enemyTarget.transform.position, _droneStationTransform.position) <
                                       item.patrolRange ;

        Func<bool> HasCollectable() => () => _collectable != null && !_isStorageFull &&
                                             Vector3.Distance(_collectable.transform.position,
                                                 _droneStationTransform.position)
                                             < item.patrolRange;
        Func<bool> HasNoTarget() => () => _enemyTarget == null;
        Func<bool> HasNoCollectable() => () =>( _collectable == null || _isStorageFull == true) || Vector3.Distance(_collectable.transform.position,
                _droneStationTransform.position)
            > item.patrolRange;
       
    }
    private void Start()
    {
        previousPosition = transform.position;
        _collectable = TargetManager.Instance.FindCollectable(gameObject.transform.position);
    }
    
    public void DroneMovement()
    {
        _stateMachine.Tick(this);
        FindEnemy();
        FindEmptyStation();
        FindCollectable();
     
    }

    public void Deviation(float leadTimePercentage)
    {
        var deviation = MathUtils.DoSin(data.deviationSpeed);
        var predictionOffset = transform.TransformDirection(deviation) * data.deviationAmount * leadTimePercentage;
        _deviatedPrediction = _standartPrediction + predictionOffset;
    }

    public void RotateDrone()
    {
        var heading = _deviatedPrediction - transform.position;
        var rotation = Quaternion.LookRotation(heading);
        _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, data.rotateSpeed * Time.deltaTime));
    }

    public void RotateDroneOnFollow()
    {
        Vector3 currentVelocity = (transform.position - previousPosition) / Time.deltaTime;
        previousPosition = transform.position;

        if (currentVelocity != Vector3.zero)
        {
            Vector3 heading = currentVelocity.normalized;
            Quaternion desiredRotation = Quaternion.LookRotation(heading, transform.up);
            targetRotation = Quaternion.Slerp(targetRotation, desiredRotation, data.rotateSpeed * Time.deltaTime);
            _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation,
                (data.rotateSpeed * 1.5f) * Time.deltaTime));
        }
    }

    private void PredictMovement(float leadTimePercentage, Rigidbody rigidbody)
    {
        var predictionTime = Mathf.Lerp(0, data.maxDistancePredict, leadTimePercentage);
        _standartPrediction = rigidbody.position +  rigidbody.velocity * predictionTime;
    }
    public void PredictMovement_EnemyTarget(float leadTimePercentage)
    {
        PredictMovement(leadTimePercentage,_enemyTarget.Rb);
        // var predictionTime = Mathf.Lerp(0, data.maxDistancePredict, leadTimePercentage);
        // _standartPrediction = _enemyTarget.Rb.position + _enemyTarget.Rb.velocity * predictionTime;
    }
    
    public void PredictMovement_Collect(float leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, data.maxDistancePredict, leadTimePercentage);
        _standartPrediction = _collectable.Rb.position + _collectable.Rb.velocity * predictionTime;
    }

    public void FindEmptyStation()
    {
        _motherShipStation = TargetManager.Instance.FindClosestStation(gameObject.transform.position);
    }

    public void FindCollectable()
    {
        _collectable = TargetManager.Instance.FindCollectable(gameObject.transform.position);
    }

    public void FindEnemy()
    {
        _enemyTarget = TargetManager.Instance.FindClosestTarget(gameObject.transform.position);
      
    }

    public void AddToStorage()
    {
        Stored++;
        if (Stored >= _storageCapacity)
        {
            _isStorageFull = true;
            
        }
    }

    public void SendCollectables()
    {
        MotherShip.Instance.AddResource(Stored);
    }

   

    public void Init(DroneStation ds)
    {
        _motherShipStation = ds;
        _droneStationTransform = ds.transform;

        TargetManager.Instance.AddDrone(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var position = transform.position;
        Gizmos.DrawLine(position, _standartPrediction);
        // Gizmos.color = Color.blue;
        // Gizmos.DrawLine(position, _collectable.transform.position);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_standartPrediction, _deviatedPrediction);
    }

    public Vector3 RandomizeDirectionMovement()
    {
        timer += Time.deltaTime;
        if (timer >= data.instance)
        {
            x = Random.Range(-3, 3);
            z = Random.Range(-3, 3);
            y = Random.Range(-10, 10);
            timer = 0f;
        }

        return new Vector3(x, y, z);
    }
}

public static class MathUtils
{
    public static Vector3 DoSin(float _deviationSpeed)
    {
        return new Vector3(Mathf.Sin(Time.time * _deviationSpeed), 0, Mathf.Cos(Time.time * _deviationSpeed));
    }
}