using System;
using UnityEngine;

namespace Upgrade
{
    [Serializable]
    public struct UpgradeGroup
    {
        public float cost;
        public float value;
    }
    [CreateAssetMenu(menuName = "Upgrade")]
    public class BaseUpgrade : ScriptableObject
    {
        public string id;
        public int level;
        public Currency currency;
    }
}
