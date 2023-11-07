using UnityEngine;

namespace Drone
{
    [CreateAssetMenu(menuName = "Scriptable Objects/DroneData")]
    public class DroneDataScriptable : ScriptableObject
    {
        public float maxDistancePredict => _maxDistancePredict;
        public float minDistancePredict => _minDistancePredict;

        public float collectRange => _collectRange;

        // public float collectTimer => _collectTimer;
        public float instance => _instance;
        public float deviationSpeed => _deviationSpeed;
        public float deviationAmount => _deviationAmount;

        public float rotateSpeed
        {
            get => _rotateSpeed;
            set => _rotateSpeed = value;
        }


        //todo: add hp, collect amount

        private float _rotateSpeed = 180;
        private float _deviationSpeed = 2;
        private float _deviationAmount = 50;
        //private float _collectTimer = 1f;
        private float _instance = 2f;
        private float _collectRange = 3f;
        private float _maxDistancePredict = 100f;
        private float _minDistancePredict = 5f;
    }
}