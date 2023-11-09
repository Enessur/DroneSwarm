using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Upgrade
{
    public class DynamicScrollView : MonoBehaviour
    {
        [SerializeField] private Transform scrollViewContent;
        [SerializeField] private UpgradePanel prefab;
        [SerializeField] private List<DroneUpgrade> droneUpgrades;
        [SerializeField] private UpgradeManager upgradeManager;
        [SerializeField] private MotherShip.MotherShip motherShip;
        
        private void Awake()
        {
            foreach (DroneUpgrade droneUpgrade in droneUpgrades)
            {
                UpgradePanel newPanel = Instantiate(prefab, scrollViewContent);
                newPanel.InitializePanel(droneUpgrade,upgradeManager,motherShip);
            }
        }
    }
}