using System;
using System.Security.Cryptography;
using Script;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class DroneAI : MonoBehaviour
{
    public enum TaskCycleDrone
    {
        Chase,
        Attack,
        Follow,
    }


    [SerializeField] private DroneScriptableObject item;
    [SerializeField] private LayerMask whatIsEnemies;
    [SerializeField] private TaskCycleDrone taskCycleDrone;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _rotateSpeed;

    
    //Prediction
    [SerializeField] private float _maxDistancePredict = 100f;
    [SerializeField] private float _minDistancePredict = 5f;
    [SerializeField] private float _maxTimePrediction = 5f;
    private Vector3 _standartPrediction, _deviatedPrediction ;
    
    //Prediction
    [SerializeField] private float _deviationAmount = 50;
    [SerializeField] private float _deviationSpeed = 2;
    
    private Quaternion targetRotation;
    private Vector3 previousPosition;
    private float timer= 0;
    private float instance= 2f;
    private float x,y,z;
    private float _findStationRange = 1f;
    private EnemyBehaviour _enemyTarget;
    private DroneStation _motherShipStation;
    private Transform _droneStationTransform;
    private DroneStation _currentStation;
    private void Start()
    {
        float c = CustomMath.AddNumbers(4, 3);
        Debug.Log(c);
        previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        DroneMovement();
    }

    private void DroneMovement()
    {
        FindEnemy();
        FindEmptyStation();


        if (_enemyTarget is not null)
        {
            if ((Vector3.Distance(_enemyTarget.transform.position, _droneStationTransform.position) < item.patrolRange)
                && (Vector3.Distance(_enemyTarget.transform.position, _droneStationTransform.position) <
                    item.droneAttackRange))
            {
                taskCycleDrone = TaskCycleDrone.Attack;
            }
            else if ((Vector3.Distance(_enemyTarget.transform.position, _droneStationTransform.position) <
                      item.patrolRange))
            {
                taskCycleDrone = TaskCycleDrone.Chase;
            }
            else
            {
                taskCycleDrone = TaskCycleDrone.Follow;
            }
        }
        else
        {
            taskCycleDrone = TaskCycleDrone.Follow;
        }


        switch (taskCycleDrone)
        {
            case TaskCycleDrone.Follow:
                transform.position = Vector3.MoveTowards(transform.position,
                    _droneStationTransform.position, item.followSpeed * Time.deltaTime);
                _rb.velocity = transform.forward * 0;

                RotateDroneOnFollow();
                break;
            case TaskCycleDrone.Attack:
                break;
            case TaskCycleDrone.Chase:

                _rb.ChangeVelocity(transform.forward * item.chaseSpeed);

                _rb.velocity += RandomizeDirectionMovement();

                var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict,
                    Vector3.Distance(transform.position, _enemyTarget.transform.position));

                PredictMovement(leadTimePercentage);
                Deviation(leadTimePercentage);
                RotateDrone();

                break;
        }
    }


    private void Deviation(float leadTimePercentage)
    {
        //var deviation = new Vector3(Mathf.Sin(Time.time * _deviationSpeed), 0,Mathf.Cos(Time.time * _deviationSpeed));
        var deviation = MathUtils.DoSin(_deviationSpeed);
        var predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;
        _deviatedPrediction = _standartPrediction + predictionOffset;
    }

    private void RotateDrone()
    {
        var heading = _deviatedPrediction - transform.position;
        var rotation = Quaternion.LookRotation(heading);
        _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation,rotation, _rotateSpeed * Time.deltaTime));
    }
    
    private void RotateDroneOnFollow()
    {
        Vector3 currentVelocity = (transform.position - previousPosition) / Time.deltaTime;
        previousPosition = transform.position;
        
        if (currentVelocity != Vector3.zero)
        {
            Vector3 heading = currentVelocity.normalized;
            Quaternion desiredRotation = Quaternion.LookRotation(heading, transform.up);
            targetRotation = Quaternion.Slerp(targetRotation, desiredRotation, _rotateSpeed * Time.deltaTime);
            _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed * Time.deltaTime));
        }
    }
    private void PredictMovement(float leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, _maxDistancePredict,leadTimePercentage);
        _standartPrediction = _enemyTarget.Rb.position + _enemyTarget.Rb.velocity * predictionTime;
    }
    public void FindEmptyStation()
    {
        _motherShipStation = TargetManager.Instance.FindClosestStation(gameObject.transform.position);
    }

    private void OnStation()
    {
        _currentStation = _motherShipStation;
    }

    public void FindEnemy()
    {
        _enemyTarget = TargetManager.Instance.FindClosestTarget(gameObject.transform.position);
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
        Gizmos.DrawLine(transform.position,_standartPrediction);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_standartPrediction,_deviatedPrediction);
    }
    public  Vector3 RandomizeDirectionMovement()
    {

        timer += Time.deltaTime;
        if (timer>= instance)
        {
            x = Random.Range(-3, 3);
            z = Random.Range(-3, 3);
            y = Random.Range(-10, 10);
         timer = 0f;
        }
        return new Vector3(x,y,z);
    }
}

public static class MathUtils 
{
    public static Vector3 DoSin(float _deviationSpeed )
    {
        return new Vector3(Mathf.Sin(Time.time * _deviationSpeed), 0,Mathf.Cos(Time.time * _deviationSpeed));
    }
}