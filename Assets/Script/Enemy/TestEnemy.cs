using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Enemy
{
    public class TestEnemy : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        public Rigidbody Rb => _rb;
        public List<TestEnemy> enemyTransforms = new();
        // private float _size =10f;
        // private float _speed =1f;
    
        void Start()
        {
            var a= LayerManager.Instance.GetLayerIndex(LayerManager.LayerType.Player);
        }

    }
}
