using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCollectState : DroneBaseState
{
    public override void EnterState(DroneAI droneAI)
    {
       Debug.Log("collect");
    }

    public override void UpdateState(DroneAI droneAI)
    {
        if (Vector3.Distance(droneAI.transform.position, droneAI._collectable.transform.position) > droneAI.collectRange)
        {
            droneAI.transform.position = Vector3.MoveTowards( droneAI.transform.position,  droneAI._collectable.transform.position,
                droneAI.item.followSpeed * Time.deltaTime);
            droneAI._rb.velocity =  droneAI.transform.forward * 0;
        }
        else
        {
            droneAI.timer += Time.deltaTime;
            if ( droneAI.timer >=  droneAI.collectTimer)
            {
                 droneAI._isStorageFull = true;
                droneAI.timer = 0;
            }
        }
    }
}
