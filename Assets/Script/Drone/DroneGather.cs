using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneGather : IState
{

    public void Tick(DroneAI droneAI)
    {
        droneAI.timer += Time.deltaTime;
        if (droneAI._isStorageFull != true)
        {
            if (droneAI.timer >= droneAI.collectTimer)
            {
                droneAI._collectable.Take();
                droneAI.AddToStorage();
                droneAI.timer = 0;
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
