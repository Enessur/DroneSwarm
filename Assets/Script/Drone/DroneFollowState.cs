using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneFollowState : IState
{
    
    
    
    public void Tick (DroneAI droneAI)
    {
        droneAI.transform.position = Vector3.MoveTowards(droneAI.transform.position,
            droneAI._droneStationTransform.position, droneAI.item.followSpeed * Time.deltaTime);
        droneAI._rb.velocity = droneAI.transform.forward * 0;
        droneAI.RotateDroneOnFollow();

        if (Vector3.Distance(droneAI.transform.position, droneAI._droneStationTransform.position) < 0.4f)
        {
            droneAI.timer += Time.deltaTime;
            if (droneAI.timer >= droneAI.data.instance)
            { 
                droneAI.SendCollectables();
                droneAI._isStorageFull = false;
                droneAI.Stored = 0;
                droneAI.timer = 0f;
            }
        }
    }
    

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }
}