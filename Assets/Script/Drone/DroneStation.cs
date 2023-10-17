using Script;
using UnityEngine;

public class DroneStation : MonoBehaviour
{
    private GameObject station;

    // private void Start()
    // {
    //     station = GameObject.FindGameObjectWithTag("Empty");
    // }

    public void Init()
    {
        TargetManager.Instance.AddDroneStation(this);
    }
    // private void OnTriggerEnter2D(Collider2D col)
    // {
    //     if (col.CompareTag("Drone"))
    //     {
    //         station.tag = "Full";
    //     }
    // }
}