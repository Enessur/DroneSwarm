using System;
using System.Collections;
using System.Collections.Generic;
using Drone;
using TMPro;
using UnityEngine;
using Upgrade;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private MotherShip _motherShip;
    [SerializeField] private DroneUpgrade _droneUpgrade;
    [SerializeField] private TMP_Text _upgradeType;
    [SerializeField] private TMP_Text _costTag;
    [SerializeField] private TMP_Text _levelText;

   // [SerializeField] private UpgradeName _upgradeName;
   
    private UpgradeGroup _upgradeGroup;

    private void Start()
    {
        _upgradeGroup = _droneUpgrade.upgrades[_droneUpgrade.level];
        _upgradeType.SetText("Upgrade " + _droneUpgrade.id + ": " + _upgradeGroup.value);
        _costTag.SetText("Upgrade Cost: " + _upgradeGroup.cost);
        _levelText.SetText("Upgrade Level: " + _droneUpgrade.level);
    }

    public void UpgradeStat()
    {
        if (_motherShip.gathered > _upgradeGroup.cost)
        {
           _motherShip.RemoveResource(_upgradeGroup.cost);
            _droneUpgrade.level++;
            _upgradeGroup = _droneUpgrade.upgrades[_droneUpgrade.level];

            _upgradeType.SetText("Upgrade " + _droneUpgrade.id + ": " + _upgradeGroup.value);
            _costTag.SetText("Upgrade Cost: " + _upgradeGroup.cost);
            _levelText.SetText("Upgrade Level: " + _droneUpgrade.level);

            _upgradeManager.Upgrade(_droneUpgrade.id, _upgradeGroup.value);
        }
    }
}