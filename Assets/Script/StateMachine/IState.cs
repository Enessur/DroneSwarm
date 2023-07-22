using System.Collections;
using System.Collections.Generic;
using Drone;
using UnityEngine;

public interface IState
{
    void Tick(DroneAI droneAI);
    void OnEnter();
    void OnExit();
    

}
