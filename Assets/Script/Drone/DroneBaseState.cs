using System.Collections;
using System.Collections.Generic;
using Drone;
using UnityEngine;

public abstract class DroneBaseState
{

   public abstract void EnterState(DroneAI droneAI);
   public abstract void UpdateState(DroneAI droneAI);
   
}
