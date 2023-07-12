using UnityEngine;

namespace Script
{
    [CreateAssetMenu(menuName = "Scriptable Objects/ Drone")]
    public class DroneScriptableObject : AgentScriptableObject
    {
        public int damage;
        public int patrolRange;
        public int droneAttackRange;
        
        
       
    }
}