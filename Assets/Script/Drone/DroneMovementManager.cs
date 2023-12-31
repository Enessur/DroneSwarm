using System;
using System.Collections;
using System.Collections.Generic;
using Drone;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DroneMovementManager : MonoBehaviour
{

    public CollisionTest collisionTest;
    public List<DroneAI> newDrone;
    

    public void AddDrone(DroneAI dr)
    {
    newDrone.Add(dr);
    }
    
    
    
    private void FixedUpdate()
    {
        DroneBehaviour();
        
    }

    private void DroneBehaviour()
    {
        foreach (var drone in newDrone)
        {
            drone.DroneMovement();
        }
    }
}