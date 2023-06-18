using UnityEngine;

namespace Script
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Agent")]
    public class AgentScriptableObject : ScriptableObject
    {
        public float chaseSpeed;
        public float followSpeed;
        
    }
}