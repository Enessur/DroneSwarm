using System;
using Drone;
using UnityEngine;

namespace Upgrade
{
    public class PlayerPrefs : MonoBehaviour
    {

        public DroneScriptableObject item;
        public DroneDataScriptable data;
        
        public float hitPoints;
        public float moveSpeed;
        public float rotateSpeed;
        public float damage;

        private void Start()
        {
            moveSpeed = item.chaseSpeed;
            rotateSpeed = data.rotateSpeed;
        }
    }
}
