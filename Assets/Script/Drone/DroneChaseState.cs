using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DroneChaseState : IState
{
    

    public void Tick(DroneAI droneAI)
    {
        droneAI._rb.ChangeVelocity(droneAI.transform.forward * droneAI.item.chaseSpeed);
        droneAI._rb.velocity += droneAI.RandomizeDirectionMovement();

        var leadTimePercentage = Mathf.InverseLerp(droneAI._minDistancePredict, droneAI._maxDistancePredict,
            Vector3.Distance(droneAI.transform.position, droneAI._enemyTarget.transform.position));

        droneAI.PredictMovement(leadTimePercentage);
        droneAI.Deviation(leadTimePercentage);
        droneAI.RotateDrone();
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }
}