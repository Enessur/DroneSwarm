using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    public List<GameObject> Enemy = new List<GameObject>();
    public List<GameObject> Collectable = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy.Add(other.gameObject);
            
        }

        if (other.CompareTag("Collectable"))
        {
            Collectable.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy.Remove(other.gameObject);
        }

        if (other.CompareTag("Collectable"))
        {
            Collectable.Remove(other.gameObject);
        }
        
    }
}