using System;
using Script;
using UnityEngine;

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
    
    
    
    private float _findStationRange = 1f;
    private TestEnemy _enemyTarget;
    private DroneStation _motherShipStation;
    private Transform _droneStationTransform;
    private DroneStation _currentStation;
    

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        FindEnemy();
        FindEmptyStation();
       // float distance = Vector2.Distance(_enemyTarget.transform.position ,transform.position);

        if (_enemyTarget is not null)
        {
            if ((Vector2.Distance(_enemyTarget.transform.position, _droneStationTransform.position) < item.patrolRange)
                && (Vector2.Distance(_enemyTarget.transform.position, _droneStationTransform.position) < item.droneAttackRange))
            {
                taskCycleDrone = TaskCycleDrone.Attack;
            }
            else if ((Vector2.Distance(_enemyTarget.transform.position, _droneStationTransform.position) < item.patrolRange))
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
                transform.position = Vector2.MoveTowards(transform.position,
                    _droneStationTransform.position, item.followSpeed * Time.deltaTime);
                _rb.velocity = transform.forward * 0;
               
                break;
            case TaskCycleDrone.Attack:
                break;
            case TaskCycleDrone.Chase:

                _rb.velocity = transform.forward * item.chaseSpeed;
                var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict,
                    Vector2.Distance(transform.position, _enemyTarget.transform.position));
                
                PredictMovement(leadTimePercentage);
                Deviation(leadTimePercentage);
                RotateDrone();
                
                // transform.position = Vector2.MoveTowards(transform.position,
                //     _enemyTarget.position, distance / 50 + (item.chaseSpeed * Time.deltaTime / 50f));
                
                break;
        }
    }



    private void Deviation(float leadTimePercentage)
    {
        var deviation = new Vector3(Mathf.Sin(Time.time * _deviationSpeed), 0,Mathf.Cos(Time.time * _deviationSpeed));
        var predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;
        _deviatedPrediction = _standartPrediction + predictionOffset;
    }

    private void RotateDrone()
    {
        var heading = _deviatedPrediction - transform.position;
        var rotation = Quaternion.LookRotation(heading);
        _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation,rotation, _rotateSpeed * Time.deltaTime));
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
}