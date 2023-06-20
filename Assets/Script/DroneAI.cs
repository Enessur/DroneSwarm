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


    private float timer= 0;
    private float instance= 2f;
    private float y;
    private float _findStationRange = 1f;
    private TestEnemy _enemyTarget;
    private DroneStation _motherShipStation;
    private Transform _droneStationTransform;
    private DroneStation _currentStation;
    private void Start()
    {
        float c = CustomMath.AddNumbers(4, 3);
        Debug.Log(c);
    }

    private void FixedUpdate()
    {
        FindEnemy();
        FindEmptyStation();
       // float distance = Vector2.Distance(_enemyTarget.transform.position ,transform.position);

        if (_enemyTarget is not null)
        {
            if ((Vector3.Distance(_enemyTarget.transform.position, _droneStationTransform.position) < item.patrolRange)
                && (Vector3.Distance(_enemyTarget.transform.position, _droneStationTransform.position) < item.droneAttackRange))
            {
                taskCycleDrone = TaskCycleDrone.Attack;
            }
            else if ((Vector3.Distance(_enemyTarget.transform.position, _droneStationTransform.position) < item.patrolRange))
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
               
                break;
            case TaskCycleDrone.Attack:
                break;
            case TaskCycleDrone.Chase:

                //_rb.velocity = transform.forward * item.chaseSpeed;
               // _rb.velocity = CustomMath.VelocityDirectionMovement( _rb ,transform.forward * item.chaseSpeed);
               //CustomMath.VelocityDirectionMovement( _rb ,transform.forward * item.chaseSpeed);
                _rb.ChangeVelocity(transform.forward * item.chaseSpeed);

                _rb.velocity += RandomizeDirectionMovement();
                
                var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict,
                    Vector3.Distance(transform.position, _enemyTarget.transform.position));
                
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
         y = Random.Range(-10, 10);
         timer = 0f;
        }
       // float x = Random.Range(5, 10);
        //float z = Random.Range(5, 10);
    
        return new Vector3(0,y,0);
    }
}

public static class MathUtils 
{
    // public static void VelocityDirectionMovement(this Rigidbody _rigidbody, Vector3 direction)
    // {
    //     _rigidbody.velocity = direction + RandomizeDirectionMovement();
    // }

    public static Vector3 DoSin(float _deviationSpeed )
    {
        return new Vector3(Mathf.Sin(Time.time * _deviationSpeed), 0,Mathf.Cos(Time.time * _deviationSpeed));
    }
  

}