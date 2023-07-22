using UnityEngine;

namespace Manager
{
    public class DebugManager : MonoBehaviour
    {
        [SerializeField] private bool m_ShowDebug;
        private static bool m_showDebug;

        private void Awake() => m_showDebug = m_ShowDebug;

        public static void CommonDebug(object obj)
        {
            if (m_showDebug)
                Debug.Log(obj);
        }

        public static void ErrorDebug(object obj)
        {
            if (m_showDebug)
                Debug.LogError(obj);
        }

    }
}
