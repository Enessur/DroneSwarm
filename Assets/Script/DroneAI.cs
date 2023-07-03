using System;
using System.Security.Cryptography;
using Script;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DroneAI : MonoBehaviour
{
    public enum TaskCycleDrone
    {
        Chase,
        Attack,
        Follow,
        GatherResource,
        Collect
    }


    public DroneBaseState currentState;
    public DroneFollowState FollowState = new DroneFollowState();
    public DroneChaseState ChaseState = new DroneChaseState();
    public DroneCollectState CollectState = new DroneCollectState();

    public Rigidbody _rb;
    public DroneScriptableObject item;
    [SerializeField] private DroneMovementManager _droneMovementManager;
    [SerializeField] private LayerMask whatIsEnemies;
    [SerializeField] private TaskCycleDrone taskCycleDrone;
    [SerializeField] private float _rotateSpeed;


    //Prediction
    public float _maxDistancePredict = 100f;
    public float _minDistancePredict = 5f;
    [SerializeField] private float _maxTimePrediction = 5f;
    [SerializeField] private float collectRange = 1f;

    private Vector3 _standartPrediction, _deviatedPrediction;

    //Prediction
    [SerializeField] public float _deviationAmount = 50;
    [SerializeField] public float _deviationSpeed = 2;

    private Quaternion targetRotation;
    private Vector3 previousPosition;
    private float timer = 0;
    private float collectTimer = 5f;
    private float instance = 2f;
    private float x, y, z;
    private float _findStationRange = 1f;
    public EnemyBehaviour _enemyTarget;
    private DroneStation _motherShipStation;
    public Transform _droneStationTransform;
    private DroneStation _currentStation;
    private Collectable _collectable;

    [FormerlySerializedAs("_isCollecting")] [SerializeField]
    private bool _isStorageFull;


    private void Start()
    {
        float c = CustomMath.AddNumbers(4, 3);
        Debug.Log(c);
        previousPosition = transform.position;
        currentState = FollowState;
        currentState.EnterState(this);
    }


    public void DroneMovement()
    {
        FindEnemy();
        FindEmptyStation();
        // FindCollectable();
        currentState.UpdateState(this);

        // if ((_enemyTarget != null))
        // {
        //     if ((Vector3.Distance(_enemyTarget.transform.position, _droneStationTransform.position) < item.patrolRange)
        //         && (Vector3.Distance(_enemyTarget.transform.position, _droneStationTransform.position) <
        //             item.droneAttackRange))
        //     {
        //         taskCycleDrone = TaskCycleDrone.Attack;
        //     }
        //
        //     else if ((Vector3.Distance(_enemyTarget.transform.position, _droneStationTransform.position) <
        //               item.patrolRange))
        //     {
        //         taskCycleDrone = TaskCycleDrone.Chase;
        //     }
        //     else
        //     {
        //         taskCycleDrone = TaskCycleDrone.Follow;
        //     }
        // }
        //
        // else
        // {
        //     taskCycleDrone = TaskCycleDrone.Follow;
        // }
        //
        //
        // switch (taskCycleDrone)
        // {
        //     case TaskCycleDrone.Follow:
        //         // Follow();
        //         // RotateDroneOnFollow();
        //         break;
        //
        //     case TaskCycleDrone.Attack:
        //         break;
        //
        //     case TaskCycleDrone.GatherResource:
        //         Collect();
        //         RotateDroneOnFollow();
        //         break;
        //
        //     case TaskCycleDrone.Collect:
        //         break;
        //
        //
        //     case TaskCycleDrone.Chase:
        //
        //         _rb.ChangeVelocity(transform.forward * item.chaseSpeed);
        //         
        //         _rb.velocity += RandomizeDirectionMovement();
        //         
        //         var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict,
        //             Vector3.Distance(transform.position, _enemyTarget.transform.position));
        //         
        //         PredictMovement(leadTimePercentage);
        //         Deviation(leadTimePercentage);
        //         RotateDrone();
        //
        //         break;
        // }
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

    // private void Follow()
    // {
    //     transform.position = Vector3.MoveTowards(transform.position,
    //         _droneStationTransform.position, item.followSpeed * Time.deltaTime);
    //     _rb.velocity = transform.forward * 0;
    //
    //     if (Vector3.Distance(transform.position, _droneStationTransform.position) < 0.4f)
    //     {
    //         timer += Time.deltaTime;
    //         if (timer >= instance)
    //         {
    //             _isStorageFull = false;
    //             timer = 0f;
    //         }
    //     }
    // }

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
    

    public void Collect()
    {
        if (Vector3.Distance(transform.position, _collectable.transform.position) > collectRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, _collectable.transform.position,
                item.followSpeed * Time.deltaTime);
            _rb.velocity = transform.forward * 0;
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= collectTimer)
            {
                _isStorageFull = true;
                timer = 0;
            }
        }
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