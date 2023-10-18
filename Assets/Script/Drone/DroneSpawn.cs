using System;
using System.Collections.Generic;
using Drone;
using UnityEngine;
using UnityEngine.Events;

public enum InputType
{
    Tap,
    Hold
}

public class DroneSpawn : MonoBehaviour
{
    [Serializable]
    private struct InputGroup
    {
        public KeyCode key;
        public UnityEvent unityEvent;
        public InputType inputType;
        public string info;
    }

    [SerializeField] private DroneAI drone;
    [SerializeField] private DroneStation droneStation;
    [SerializeField] private MotherShip motherShip;
    [SerializeField] private List<DroneStation> droneStations;
    [SerializeField] private List<InputGroup> inputGroups;
    public float radius;
    private float addRadius;
    private float _rotationSpeed;
    private float VecY = 2f;
    private DroneMovementManager m_droneMovementManager;

    private void Start()
    {
        m_droneMovementManager = GetComponent<DroneMovementManager>();
        _rotationSpeed = 50f;

#if UNITY_EDITOR
        LogInputs();
#endif
    }

    private void Update()
    {
        CheckInputs();
        RotateObjects();
    }

    private void CheckInputs()
    {
        foreach (var inputGroup in inputGroups)
        {
            switch (inputGroup.inputType)
            {
                case InputType.Hold:
                    if (Input.GetKey(inputGroup.key))
                    {
                        inputGroup.unityEvent?.Invoke();
                    }

                    break;

                case InputType.Tap:
                    if (Input.GetKeyDown(inputGroup.key))
                    {
                        inputGroup.unityEvent?.Invoke();
                    }

                    break;
            }
        }
    }

    public void SpawnStation()
    {
        var ds = Instantiate(droneStation, GetSpawnPosition(), Quaternion.identity);
        var dr = Instantiate(drone, GetSpawnPosition(), Quaternion.identity);
        dr.Init(ds);
        ds.Init();
        droneStations.Add(ds);
        ds.transform.SetParent(motherShip.transform);
        float angleIncrement = 360f / droneStations.Count;
        for (int i = 0; i < droneStations.Count; i++)
        {
            float angle = i * angleIncrement;
            Vector3 newPosition = GetCirclePosition(angle);
            if (i % 2 == 0)
            {
                newPosition.y = VecY;
            }

            droneStations[i].transform.position = newPosition;
        }

        m_droneMovementManager.AddDrone(dr);
    }

   

    private void RotateObjects()
    {
        foreach (var t in droneStations)
        {
            t.transform
                .RotateAround(motherShip.transform.position, Vector3.up, _rotationSpeed * Time.deltaTime);
        }
    }

    private Vector3 GetSpawnPosition()
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.z += radius;
        return spawnPosition;
    }

    private Vector3 GetCirclePosition(float angle)
    {
        addRadius = +droneStations.Count * 0.1f;


        float x = Mathf.Sin(Mathf.Deg2Rad * angle) * (radius + addRadius);
        float z = Mathf.Cos(Mathf.Deg2Rad * angle) * (radius + addRadius);
        return motherShip.transform.position + new Vector3(x, 0f, z);
    }

    public void LogInputs()
    {
        string logString = "";
        foreach (var inputGroup in inputGroups)
        {
            logString += inputGroup.key + " -> " + inputGroup.info + "\n";
        }
    }
}