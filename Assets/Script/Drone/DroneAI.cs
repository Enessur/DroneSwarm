using System;
using System.Security.Cryptography;
using Script;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DroneAI : MonoBehaviour
{
    public DroneBaseState currentState;
    public DroneFollowState FollowState = new DroneFollowState();
    public DroneChaseState ChaseState = new DroneChaseState();
    public DroneCollectState CollectState = new DroneCollectState();

    public Rigidbody _rb;
    public DroneScriptableObject item;
    
    public float _maxDistancePredict = 100f;
    public float _minDistancePredict = 5f;
    public float collectRange = 3f;
    public int Stored = 0;
    public float timer = 0;
    public float collectTimer = 1f;
    public float instance = 2f;
    public Collectable _collectable;
    public EnemyBehaviour _enemyTarget;
    public Transform _droneStationTransform;
    [SerializeField] public float _deviationSpeed = 2;
    [SerializeField] public float _deviationAmount = 50;
    private int _storageLimit;
    private Vector3 _standartPrediction, _deviatedPrediction;
    // [SerializeField] private float _maxTimePrediction = 5f;


    //Prediction
    [SerializeField] private DroneMovementManager _droneMovementManager;
    [SerializeField] private LayerMask whatIsEnemies;
    [SerializeField] private float _rotateSpeed;
    private Quaternion targetRotation;
    private Vector3 previousPosition;
    [SerializeField] private  int storageCapacity = 20;
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
        Func<bool> HasNoCollectable() => () => _collectable == null;
    }

    private void Start()
    {
        previousPosition = transform.position;
        // currentState.EnterState(this);
        _collectable = TargetManager.Instance.FindCollectable(gameObject.transform.position);
    }


    // public void StateManager()
    // {
    //     if (_enemyTarget != null && Vector3.Distance(_enemyTarget.transform.position, _droneStationTransform.position) < item.patrolRange && currentState != ChaseState)
    //     {
    //         SwitchState(ChaseState);
    //         return;
    //     }
    //
    //     if (_collectable != null && _isStorageFull != true &&
    //         Vector3.Distance(_collectable.transform.position, _droneStationTransform.position) < item.patrolRange)
    //     {
    //         if (currentState != CollectState)
    //         {
    //             SwitchState(CollectState);
    //         }
    //
    //         return;
    //     }
    //
    //     if (currentState != FollowState)
    //     {
    //         SwitchState(FollowState);
    //     }
    // }

    public void DroneMovement()
    {
        _stateMachine.Tick(this);
        FindEnemy();
        FindEmptyStation();
        // StateManager();
        FindCollectable();
        
        
        
        // currentState.UpdateState(this);
    }

    public void SwitchState(DroneBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }


    public void Deviation(float leadTimePercentage)
    {
        var deviation = MathUtils.DoSin(_deviationSpeed);
        var predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;
        _deviatedPrediction = _standartPrediction + predictionOffset;
    }

    public void RotateDrone()
    {
        var heading = _deviatedPrediction - transform.position;
        var rotation = Quaternion.LookRotation(heading);
        _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _rotateSpeed * Time.deltaTime));
    }

    public void RotateDroneOnFollow()
    {
        Vector3 currentVelocity = (transform.position - previousPosition) / Time.deltaTime;
        previousPosition = transform.position;

        if (currentVelocity != Vector3.zero)
        {
            Vector3 heading = currentVelocity.normalized;
            Quaternion desiredRotation = Quaternion.LookRotation(heading, transform.up);
            targetRotation = Quaternion.Slerp(targetRotation, desiredRotation, _rotateSpeed * Time.deltaTime);
            _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation,
                (_rotateSpeed * 1.5f) * Time.deltaTime));
        }
    }

    public void PredictMovement(float leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, _maxDistancePredict, leadTimePercentage);
        _standartPrediction = _enemyTarget.Rb.position + _enemyTarget.Rb.velocity * predictionTime;
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
        if (Stored >= storageCapacity)
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
        if (timer >= instance)
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