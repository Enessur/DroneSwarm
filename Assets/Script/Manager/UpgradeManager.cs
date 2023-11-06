using System;
using System.Collections;
using System.Collections.Generic;
using Drone;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private DroneScriptableObject _droneData;
    [SerializeField] private DroneDataScriptable _droneDataSc;
   

    public void Upgrade(string id, float value)
    {
        switch (id)
        {
            case "Speed":
                _droneData.chaseSpeed = value;
                break;
            case "RotateSpeed":
                _droneDataSc.rotateSpeed = value;
                break;
        }
    }
}