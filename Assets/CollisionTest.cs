using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{
  
    public List<GameObject> liste = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        liste.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        liste.Remove(other.gameObject);
    }
}
