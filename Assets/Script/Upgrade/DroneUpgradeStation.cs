using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Upgrade;

public class DroneUpgradeStation : MonoBehaviour
{
    [SerializeField] private Popup _popup;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _popup.Open();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _popup.Close();
        }
    }
}