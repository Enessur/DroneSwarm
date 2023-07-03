using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneChaseState : DroneBaseState
{
    public override void EnterState(DroneAI droneAI)
    {
        Debug.Log("Chase");
    }

    public override void UpdateState(DroneAI droneAI)
    {
        if (droneAI._enemyTarget != null)
        {
            droneAI._rb.ChangeVelocity(droneAI.transform.forward * droneAI.item.chaseSpeed);

            droneAI._rb.velocity += droneAI.RandomizeDirectionMovement();

            var leadTimePercentage = Mathf.InverseLerp(droneAI._minDistancePredict, droneAI._maxDistancePredict,
                Vector3.Distance(droneAI.transform.position, droneAI._enemyTarget.transform.position));

            droneAI.PredictMovement(leadTimePercentage);
            droneAI.Deviation(leadTimePercentage);
            droneAI.RotateDrone();
        }
        else
        {
            droneAI.SwitchState(droneAI.FollowState);
        }
    }
}