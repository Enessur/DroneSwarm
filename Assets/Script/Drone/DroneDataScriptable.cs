using UnityEngine;

namespace Drone
{
    [CreateAssetMenu(menuName = "Scriptable Objects/DroneData")]
    public class DroneDataScriptable : ScriptableObject
    {



        public float maxDistancePredict => _maxDistancePredict; 
        public float minDistancePredict => _minDistancePredict;
        public float collectRange =>_collectRange;
        // public float collectTimer => _collectTimer;
        public float instance => _instance;
        public float deviationSpeed => _deviationSpeed;
        public float deviationAmount => _deviationAmount;

        public float rotateSpeed => _rotateSpeed;

        
        

        [SerializeField] private float _rotateSpeed = 180;
        [SerializeField] private float _deviationSpeed = 2;
        [SerializeField] private float _deviationAmount = 50;
        [SerializeField] private float _collectTimer = 1f;
        [SerializeField] private float _instance = 2f;
        [SerializeField] private float _collectRange =3f;
        [SerializeField] private float _maxDistancePredict = 100f;
        [SerializeField] private float _minDistancePredict = 5f;
    
    }
}
