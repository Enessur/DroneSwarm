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
    [SerializeField] private DroneMovementManager _droneMovementManager;
    [SerializeField] private LayerMask whatIsEnemies;
    [SerializeField] private float _rotateSpeed;


    //Prediction
    public float _maxDistancePredict = 100f;
    public float _minDistancePredict = 5f;
    public float collectRange = 1f;
    [SerializeField] private float _maxTimePrediction = 5f;

    private Vector3 _standartPrediction, _deviatedPrediction;

    //Prediction
    [SerializeField] public float _deviationAmount = 50;
    [SerializeField] public float _deviationSpeed = 2;

    private Quaternion targetRotation;
    private Vector3 previousPosition;
    public float timer = 0;
    public float collectTimer = 5f;
    public float instance = 2f;
    private float x, y, z;
    private float _findStationRange = 1f;
    public EnemyBehaviour _enemyTarget;
    private DroneStation _motherShipStation;
    public Transform _droneStationTransform;
    private DroneStation _currentStation;
    public Collectable _collectable;

    [FormerlySerializedAs("_isCollecting")] [SerializeField]
    public bool _isStorageFull;
    

    private void Start()
    {
        float c = CustomMath.AddNumbers(4, 3);
        Debug.Log(c);
        previousPosition = transform.position;
        currentState = FollowState;
        currentState.EnterState(this);
    }


    public void StateManager()
    {
        if (_enemyTarget is not null)
        {
            if ((Vector3.Distance(_enemyTarget.transform.position, _droneStationTransform.position) <
                 item.patrolRange))
            {
                if (currentState != ChaseState)
                {
                    SwitchState(ChaseState);
                }
            }

            // if ((Vector3.Distance(_collectable.transform.position, _droneStationTransform.position) < item.patrolRange))
            // {
            //    
            //         SwitchState(CollectState);
            //   
            // }
            else
            {
                if (currentState != FollowState)
                {
                    SwitchState(FollowState);
                }
            }
        }
        else
        {
            if (currentState != FollowState)
            {
                SwitchState(FollowState);
            }
        }
    }

    public void DroneMovement()
    {
        FindEnemy();
        FindEmptyStation();
        StateManager();
        FindCollectable();
        currentState.UpdateState(this);
    }

    public void SwitchState(DroneBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }


    public void Deviation(float leadTimePercentage)
    {
        //var deviation = new Vector3(Mathf.Sin(Time.time * _deviationSpeed), 0,Mathf.Cos(Time.time * _deviationSpeed));
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
        // if (Vector3.Distance(transform.position , _enemyTarget.transform.position)  > item.patrolRange)
        // {
        //     _enemyTarget = null;
        // }
    }


    public void Collect()
    {
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