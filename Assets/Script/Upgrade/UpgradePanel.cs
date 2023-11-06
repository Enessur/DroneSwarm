using System;
using System.Collections;
using System.Collections.Generic;
using Drone;
using UnityEngine;
using Upgrade;

public class UpgradePanel : MonoBehaviour
{
    
    [SerializeField] private DroneScriptableObject _droneData;
    [SerializeField] private MotherShip _motherShip;
    [SerializeField] private DroneUpgrade _droneUpgrade;
    [SerializeField] private string levelId;
    [SerializeField] private int levelIndex;
    private void Awake()
    {
        levelId = _droneUpgrade.id;
        levelIndex = _droneUpgrade.level;
    }

    private void Start()
    {
        UpgradeGroup upradeLevel = _droneUpgrade.upgrades[levelIndex];
        Debug.Log("Drone Speed Upgrade:"+levelIndex);
        Debug.Log("Value: " + upradeLevel.value);
        Debug.Log("Cost: " + upradeLevel.cost);
    }

    public void UpgradeStat()
    {
        levelIndex++;
        UpgradeGroup upradeLevel = _droneUpgrade.upgrades[levelIndex];
        Debug.Log("Drone Speed Upgrade:"+levelIndex);
        Debug.Log("Value: " + upradeLevel.value);
        Debug.Log("Cost: " + upradeLevel.cost);
        _droneUpgrade.level = levelIndex;
    }
}