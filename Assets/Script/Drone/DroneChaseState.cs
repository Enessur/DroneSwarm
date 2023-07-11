using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DroneChaseState : DroneBaseState
{
    public override void EnterState(DroneAI droneAI)
    {
    }

    public override void UpdateState(DroneAI droneAI)
    {
        
            droneAI._rb.ChangeVelocity(droneAI.transform.forward * droneAI.item.chaseSpeed);
            droneAI._rb.velocity += droneAI.RandomizeDirectionMovement();

            var leadTimePercentage = Mathf.InverseLerp(droneAI._minDistancePredict, droneAI._maxDistancePredict,
                Vector3.Distance(droneAI.transform.position, droneAI._enemyTarget.transform.position));

            droneAI.PredictMovement(leadTimePercentage);
            droneAI.Deviation(leadTimePercentage);
            droneAI.RotateDrone();
            
    }
}