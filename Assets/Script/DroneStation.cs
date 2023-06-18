using Script;
using UnityEngine;

public class DroneStation : MonoBehaviour
{
    private GameObject station;

    private void Start()
    {
        Debug.Log(transform);
        station = GameObject.FindGameObjectWithTag("Empty");
    }

    public void Init()
    {
        TargetManager.Instance.AddDroneStation(this);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Drone"))
        {
            Debug.Log("This Station is Full!");
            station.tag = "Full";
        }
    }
}