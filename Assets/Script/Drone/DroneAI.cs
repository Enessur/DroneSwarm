using System;
using Manager;
using Script;
using UnityEngine;
using UnityEngine.Serialization;
using Upgrade;
using PlayerPrefs = Upgrade.PlayerPrefs;
using Random = UnityEngine.Random;

namespace Drone
{
    public class DroneAI : MonoBehaviour
    {
        public PlayerPrefs playerPrefs;
        public Rigidbody rb;
        public DroneScriptableObject item;
        public DroneDataScriptable data;
        public Collectable collectable;
        public EnemyBehaviour enemyTarget;
        public Transform droneStationTransform;
        public float collectTimer = 1f;

        [SerializeField] private int storageCapacity = 20;
        // [SerializeField] private float moveSpeed;
        // [SerializeField] private float rotateSpeed;


        public float timer { get; set; } = 0;
        public int Stored { get; set; } = 0;

        private Vector3 _standartPrediction, _deviatedPrediction;
        // [SerializeField] private float _maxTimePrediction = 5f;

        //Prediction
        private Quaternion targetRotation;
        private Vector3 previousPosition;

        private float x, y, z;

        // private float _findStationRange = 1f;
        private DroneStation _motherShipStation;
        private DroneStation _currentStation;
        private StateMachine.StateMachine _stateMachine;

        [FormerlySerializedAs("_isCollecting")] [SerializeField]
        public bool _isStorageFull;

        public void Init(DroneStation ds, UpgradeManager upgradeManager)
        {
            playerPrefs.moveSpeed = upgradeManager.GetUpgradeData(DroneUpgrade.DroneUpgradeType.MoveSpeed);
            playerPrefs.rotateSpeed = upgradeManager.GetUpgradeData(DroneUpgrade.DroneUpgradeType.RotateSpeed);
            _motherShipStation = ds;
            droneStationTransform = ds.transform;
            _stateMachine = new StateMachine.StateMachine();

            var chase = new DroneChaseState();
            var follow = new DroneFollowState();
            var collect = new DroneCollectState();

            At(follow, chase, HasTarget());
            At(collect, chase, HasTarget());
            At(follow, collect, HasCollectable());
            At(chase, follow, HasNoTarget());
            At(collect, follow, HasNoCollectable());

            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

            _stateMachine.SetState(follow);

            Func<bool> HasTarget() => () => enemyTarget != null &&
                                            Vector3.Distance(enemyTarget.transform.position,
                                                droneStationTransform.position) <
                                            item.patrolRange;

            Func<bool> HasCollectable() => () => collectable != null && !_isStorageFull &&
                                                 Vector3.Distance(collectable.transform.position,
                                                     droneStationTransform.position)
                                                 < item.patrolRange;

            Func<bool> HasNoTarget() => () => enemyTarget == null;

            Func<bool> HasNoCollectable() => () => (collectable == null || _isStorageFull) || Vector3.Distance(
                    collectable.transform.position,
                    droneStationTransform.position)
                > item.patrolRange;

            upgradeManager.onUpgrade.AddListener(OnUpgrade);

            previousPosition = transform.position;
            collectable = TargetManager.Instance.FindCollectable(gameObject.transform.position);
            TargetManager.Instance.AddDrone(this);
        }


        private void OnUpgrade(DroneUpgrade.DroneUpgradeType type, float value)
        {
            switch (type)
            {
                case DroneUpgrade.DroneUpgradeType.MoveSpeed:
                    playerPrefs.moveSpeed = value;
                    break;
                case DroneUpgrade.DroneUpgradeType.RotateSpeed:
                    playerPrefs.rotateSpeed = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
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
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, playerPrefs.rotateSpeed * Time.deltaTime));
        }

        public void RotateDroneOnFollow()
        {
            Vector3 currentVelocity = (transform.position - previousPosition) / Time.deltaTime;
            previousPosition = transform.position;

            if (currentVelocity != Vector3.zero)
            {
                Vector3 heading = currentVelocity.normalized;
                Quaternion desiredRotation = Quaternion.LookRotation(heading, transform.up);
                targetRotation = Quaternion.Slerp(targetRotation, desiredRotation, playerPrefs.rotateSpeed * Time.deltaTime);
                rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation,
                    (playerPrefs.rotateSpeed * 1.5f) * Time.deltaTime));
            }
        }

        private void PredictMovement(float leadTimePercentage, Rigidbody rigidbody)
        {
            var predictionTime = Mathf.Lerp(0, data.maxDistancePredict, leadTimePercentage);
            _standartPrediction = rigidbody.position + rigidbody.velocity * predictionTime;
        }

        public void PredictMovement_EnemyTarget(float leadTimePercentage)
        {
            PredictMovement(leadTimePercentage, enemyTarget.Rb);
            // var predictionTime = Mathf.Lerp(0, data.maxDistancePredict, leadTimePercentage);
            // _standartPrediction = _enemyTarget.Rb.position + _enemyTarget.Rb.velocity * predictionTime;
        }

        public void PredictMovement_Collect(float leadTimePercentage)
        {
            var predictionTime = Mathf.Lerp(0, data.maxDistancePredict, leadTimePercentage);
            _standartPrediction = collectable.Rb.position + collectable.Rb.velocity * predictionTime;
        }

        public void FindEmptyStation()
        {
            _motherShipStation = TargetManager.Instance.FindClosestStation(gameObject.transform.position);
        }

        public void FindCollectable()
        {
            collectable = TargetManager.Instance.FindCollectable(gameObject.transform.position);
        }

        public void FindEnemy()
        {
            enemyTarget = TargetManager.Instance.FindClosestTarget(gameObject.transform.position);
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
            MotherShip.MotherShip.Instance.AddResource(Stored);
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
                y = Random.Range(-20, 20);
                timer = 0f;
            }

            return new Vector3(x, y, z);
        }
    }

    public static class MathUtils
    {
        public static Vector3 DoSin(float deviationSpeed)
        {
            return new Vector3(Mathf.Sin(Time.time * deviationSpeed), 0, Mathf.Cos(Time.time * deviationSpeed));
        }
    }
}