using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneFollowState : DroneBaseState
{
    
     
    public override void EnterState(DroneAI droneAI)
    {
        Debug.Log("Follow");
       
    }

    public override void UpdateState(DroneAI droneAI)
    {
        if (droneAI._enemyTarget == null)
        {
            droneAI.transform.position = Vector3.MoveTowards(droneAI.transform.position,
                droneAI._droneStationTransform.position, droneAI.item.followSpeed * Time.deltaTime);
            droneAI._rb.velocity = droneAI.transform.forward * 0;
            droneAI.RotateDroneOnFollow();
        }
        else
        {
            droneAI.SwitchState(droneAI.ChaseState);
        }

    }
}
