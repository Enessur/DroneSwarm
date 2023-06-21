using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInharitance : EnemyBehaviour
{
     public enum Inharitance
     {
          Teleport,
     }

     [SerializeField] private Inharitance ınh;
     private float tp = 2f;

     protected override void Update()
     {
          base.Update();

          if (Vector3.Distance(transform.position, _target.position) < tp)
          {
               ınh = Inharitance.Teleport;
          }
          
          switch (ınh)
          {
               case Inharitance.Teleport:
                
                    break;
          }
     }
}
