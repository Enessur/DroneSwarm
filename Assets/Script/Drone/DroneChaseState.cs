using UnityEngine;

namespace Drone
{
    public class DroneChaseState : IState
    {
        public void Tick(DroneAI droneAI)
        {
            droneAI.rb.ChangeVelocity(droneAI.transform.forward * droneAI.item.chaseSpeed);
            droneAI.rb.velocity += droneAI.RandomizeDirectionMovement();

            var leadTimePercentage = Mathf.InverseLerp(droneAI.data.minDistancePredict, droneAI.data.maxDistancePredict,
                Vector3.Distance(droneAI.transform.position, droneAI.enemyTarget.transform.position));

            droneAI.PredictMovement_EnemyTarget(leadTimePercentage);
            droneAI.Deviation(leadTimePercentage);
            droneAI.RotateDrone();
        }

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }
    }
}