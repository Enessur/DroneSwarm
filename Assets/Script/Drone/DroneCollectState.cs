using UnityEngine;

namespace Drone
{
    public class DroneCollectState : IState
    {

        public void Tick(DroneAI droneAI)
        {
        
        
        
            if (Vector3.Distance(droneAI.transform.position, droneAI.collectable.transform.position) >
                droneAI.data.collectRange)
            {
                droneAI.rb.ChangeVelocity(droneAI.transform.forward * droneAI.item.chaseSpeed);
                droneAI.rb.velocity += droneAI.RandomizeDirectionMovement();

                var leadTimePercentage = Mathf.InverseLerp(droneAI.data.minDistancePredict, droneAI.data.maxDistancePredict,

                    Vector3.Distance(droneAI.transform.position, droneAI.collectable.transform.position));

                droneAI.PredictMovement_Collect(leadTimePercentage);
                droneAI.Deviation(leadTimePercentage);
                droneAI.RotateDrone();
            }
            else
            {
                droneAI.timer += Time.deltaTime;
                if (droneAI._isStorageFull != true)
                {
                    droneAI.rb.velocity = droneAI.transform.forward * 0;
                    if (droneAI.timer >= droneAI.collectTimer)
                    {
                        droneAI.collectable.Take();
                        droneAI.AddToStorage();
                        droneAI.timer = 0;
                    
                    }
                }
            }

            droneAI.RotateDrone();    }

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }
    }
}