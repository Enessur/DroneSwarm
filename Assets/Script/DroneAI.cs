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

    private float _findStationRange = 1f;
    private Transform _enemyTarget;
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
        float distance = Vector2.Distance(_enemyTarget.position ,transform.position);

        if (_enemyTarget is not null)
        {
            if ((Vector2.Distance(_enemyTarget.position, _droneStationTransform.position) < item.patrolRange)
                && (Vector2.Distance(_enemyTarget.position, _droneStationTransform.position) < item.droneAttackRange))
            {
                taskCycleDrone = TaskCycleDrone.Attack;
            }
            else if ((Vector2.Distance(_enemyTarget.position, _droneStationTransform.position) < item.patrolRange))
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
                // if (Vector2.Distance(transform.position, _droneStationTransform.position) < _findStationRange)
                // {
                //     OnStation();
                //     transform.position =
                //         Vector2.MoveTowards(transform.position, _currentStation.transform.position, chaseSpeed * Time.deltaTime);
                // }
                break;
            case TaskCycleDrone.Attack:
                break;
            case TaskCycleDrone.Chase:
              
                transform.position = Vector2.MoveTowards(transform.position,
                    _enemyTarget.position, distance/50 + (item.chaseSpeed * Time.deltaTime/50f));
                
                break;
        }
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
}