using Script;
using UnityEngine;

public class DroneStation : MonoBehaviour
{
    private GameObject station;
    public void Init()
    {
        TargetManager.Instance.AddDroneStation(this);
    }
   
}