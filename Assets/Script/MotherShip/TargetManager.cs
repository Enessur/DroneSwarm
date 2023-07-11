using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class TargetManager : Singleton<TargetManager>
    {
        public List<DroneAI> droneAI = new();
        public List<EnemyBehaviour> enemyTransforms = new();
        public List<DroneStation> droneStations = new();
        public List<Collectable> collectable = new();

        public DroneStation closestStation;
        public EnemyBehaviour closestEnemy;
        public Collectable closestCollectable;
        private float detectionDistance = 30f;
        private Vector3 _offset;
        private float _currentDistance;
        private float _closestDistance;


        public void AddCollectable(Collectable cl)
        {
            collectable.Add(cl);
        }

        public void RemoveCollectable(Collectable cl)
        {
            collectable.Remove(cl);
        }
        
        
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
    
        public void AddEnemy(EnemyBehaviour tr)
        {
            enemyTransforms.Add(tr);
        }

        public void RemoveEnemy(EnemyBehaviour tr)
        {
            enemyTransforms.Remove(tr);
        
        }
   


        public EnemyBehaviour FindClosestTarget(Vector3 enemyPosition)
        {
            if (enemyTransforms.Count != 0)
            {
                _closestDistance = 30f;
                foreach (var t in enemyTransforms)
                {
                    {
                        _offset = enemyPosition - t.gameObject.transform.position;
                        _currentDistance = Vector3.Magnitude(_offset);

                        if (_closestDistance > _currentDistance)
                        {
                            _closestDistance = _currentDistance;
                            closestEnemy = t;
                        }
                    }
                }

                if (_closestDistance < detectionDistance)
                {
                    return closestEnemy;
                }

                //todo: game over ! 
            
                return null;
            }

            return null;
        }
        
        public Collectable FindCollectable(Vector3 collectablePosition)
        {
            if (collectable.Count != 0)
            {
                _closestDistance = 100000f;
                foreach (var t in collectable)
                {
                    {
                        _offset = collectablePosition - t.gameObject.transform.position;
                        _currentDistance = Vector3.Magnitude(_offset);

                        if (_closestDistance > _currentDistance)
                        {
                            _closestDistance = _currentDistance;
                            closestCollectable = t;
                        }
                    }
                }

                if (_closestDistance < detectionDistance)
                {
                    return closestCollectable;
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
                
                //TODO : We will change this to Job System.
                
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