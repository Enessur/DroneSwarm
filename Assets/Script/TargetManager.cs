using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class TargetManager : Singleton<TargetManager>
    {
        public List<DroneAI> droneAI = new();
        public List<Transform> enemyTransforms = new();
        public List<DroneStation> droneStations = new();
        
        public DroneStation closestStation;
        public GameObject closestEnemy;
        private float detectionDistance = 100f;
        private Vector3 _offset;
        private float _currentDistance;
        private float _closestDistance;
    

        public void AddDroneStation(DroneStation ds)
        {
            droneStations.Add(ds);
        }
        public void RemoveDroneStation(DroneStation ds)
        {
            droneStations.Remove(ds);
        }
        public void AddDrone(DroneAI ds)
        {
            droneAI.Add(ds);
        }
        public void RemoveDrone(DroneAI ds)
        {
            droneAI.Remove(ds);
        }
    
        public void AddEnemy(Transform tr)
        {
            enemyTransforms.Add(tr);
        }

        public void RemoveEnemy(Transform tr)
        {
            enemyTransforms.Remove(tr);
        
        }
   


        public Transform FindClosestTarget(Vector3 swordPosition)
        {
            if (enemyTransforms.Count != 0)
            {
                _closestDistance = 100000f;
                foreach (var t in enemyTransforms)
                {
                    {
                        _offset = swordPosition - t.gameObject.transform.position;
                        _currentDistance = Vector3.Magnitude(_offset);

                        if (_closestDistance > _currentDistance)
                        {
                            _closestDistance = _currentDistance;
                            closestEnemy = t.gameObject;
                        }
                    }
                }

                if (_closestDistance < detectionDistance)
                {
                    return closestEnemy.transform;
                }

                //todo: game over ! 
            
                return null;
            }

            return null;
        }
        
        public DroneStation FindClosestStation(Vector3 dronePosition)
        {
            if (droneStations.Count != 0)
            {
                _closestDistance = 100000f;
                foreach (var t in droneStations)
                {
                    {
                        _offset = dronePosition - t.gameObject.transform.position;
                        _currentDistance = Vector3.Magnitude(_offset);

                        if (_closestDistance > _currentDistance)
                        {
                            _closestDistance = _currentDistance;
                            closestStation = t;
                        }
                    }
                }

                if (_closestDistance < detectionDistance)
                {
                    return closestStation;
                }

                return null;
            }

            return null;
        }
     
     
    
    }
}