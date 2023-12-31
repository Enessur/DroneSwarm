using UnityEngine;

namespace Drone
{
    public class DroneFollowState : IState
    {
        public void Tick (DroneAI droneAI)
        {
            droneAI.transform.position = Vector3.MoveTowards(droneAI.transform.position,
                droneAI.droneStationTransform.position, droneAI.playerPrefs.moveSpeed * Time.deltaTime);
            droneAI.rb.velocity = droneAI.transform.forward * 0;
            droneAI.RotateDroneOnFollow();

            if ((Vector3.Distance(droneAI.transform.position, droneAI.droneStationTransform.position) < 0.4f)&&(droneAI.Stored > 0))
            {
                droneAI.timer += Time.deltaTime;
                if (droneAI.timer >= droneAI.data.instance)
                { 
                    droneAI.SendCollectables();
                    droneAI.isStorageFull = false;
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
}