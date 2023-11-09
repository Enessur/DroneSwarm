using System.Collections.Generic;
using Drone;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Upgrade;

namespace Manager
{
    public class UpgradeManager : MonoBehaviour
    {
        public class DroneUpgradeGroup
        {
            public DroneUpgrade droneUpgrade;
            public int index;
        }

        [SerializeField] private DroneScriptableObject droneData;
        [SerializeField] private DroneDataScriptable droneDataSc;
        [SerializeField] private DroneUpgrade[] droneUpgrades;
        [ShowInInspector] private Dictionary<DroneUpgrade.DroneUpgradeType, DroneUpgradeGroup> _droneUpgradeDic = new();
        public UnityEvent<DroneUpgrade.DroneUpgradeType,float> onUpgrade;
    

        private void Awake()
        {
            InitDictionary();
        }

    
        private void InitDictionary()
        {
            foreach (var droneUpgrade in droneUpgrades)
            {
                var group = new DroneUpgradeGroup
                {
                    droneUpgrade = droneUpgrade,
                    index = 0
                };
                _droneUpgradeDic.Add(droneUpgrade.droneUpgradeType, group);
            }
        }

        public void Upgrade(DroneUpgrade.DroneUpgradeType type, int value)
        {
            var upgradeData = _droneUpgradeDic[type];
            upgradeData.index = value;
            Debug.Log(type + " " + value);
            onUpgrade?.Invoke(type,upgradeData.droneUpgrade.upgrades[value].value);
        }

        public float GetUpgradeData(DroneUpgrade.DroneUpgradeType type)
        {

            var data = _droneUpgradeDic[type];
            return data.droneUpgrade.upgrades[data.index].value;
    }
    }
}