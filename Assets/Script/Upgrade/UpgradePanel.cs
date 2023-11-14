using Manager;
using TMPro;
using UnityEngine;

namespace Upgrade
{
    public class UpgradePanel : MonoBehaviour
    {
        [SerializeField] private UpgradeManager upgradeManager;
        [SerializeField] private MotherShip.MotherShip motherShip;
        [SerializeField] private DroneUpgrade droneUpgrade;
        [SerializeField] private TMP_Text upgradeType;
        [SerializeField] private TMP_Text costTag;
        [SerializeField] private TMP_Text levelText;
        private int _index;

        // [SerializeField] private UpgradeName _upgradeName;

        private UpgradeGroup _upgradeGroup;

        private void Start()
        {
           
          
        }

        public void UpgradeStat()
        {
            if (motherShip.gathered > _upgradeGroup.cost)
            {
                motherShip.RemoveResource(_upgradeGroup.cost);
                _index++;
                _upgradeGroup = droneUpgrade.upgrades[_index];

                upgradeType.SetText("Upgrade " + droneUpgrade.id + ": " + _upgradeGroup.value);
                costTag.SetText("Upgrade Cost: " + _upgradeGroup.cost);
                levelText.SetText("Upgrade Level: " + _index);
                upgradeManager.Upgrade(droneUpgrade.droneUpgradeType, _index);
            }
        }
    
        public void InitializePanel(DroneUpgrade setDroneUpgrade,UpgradeManager manager,MotherShip.MotherShip ms)
        {
            upgradeManager = manager;
            motherShip = ms;
            droneUpgrade = setDroneUpgrade;
            _upgradeGroup = droneUpgrade.upgrades[droneUpgrade.level];
            upgradeType.SetText("Upgrade " + droneUpgrade.id + ": " + _upgradeGroup.value);
            costTag.SetText("Upgrade Cost: " + _upgradeGroup.cost);
            levelText.SetText("Upgrade Level: " + droneUpgrade.level);
        }
    }
}