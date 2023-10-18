using System;
using System.Collections;
using System.Collections.Generic;
using Drone;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private MotherShip _motherShip;
    [SerializeField] private DroneScriptableObject _droneData;
    [SerializeField] private Text _upgradeType;
    [SerializeField] private Text _costTag;
    
    
    private int _collectableGold;
    private int _cost =100;

    private void Start()
    {
        _costTag.text = "Upgrade Cost = " + _cost;
        _upgradeType.text = "Speed Upgrade ="+_droneData.chaseSpeed;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            UpgradeSpeed();
        }
    }

    public void UpgradeSpeed()
    {
        _collectableGold = _motherShip.gathered;
        if (_collectableGold > _cost)
        {
            _droneData.chaseSpeed =_droneData.chaseSpeed+ 10;
            _motherShip.gathered = _motherShip.gathered - _cost;
            _cost = _cost+100;
            Debug.Log("ben upgrade oluyom"+_droneData.chaseSpeed);
            
        }
        _costTag.text = "Upgrade Cost = " + _cost;
        _upgradeType.text = "Speed Upgrade ="+_droneData.chaseSpeed;

    }
}
