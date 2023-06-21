using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInharitance : EnemyBehaviour
{
     public enum Inharitance
     {
          Teleport,
     }
     [SerializeField] private NewEnemyTypeScriptable item2;
     [SerializeField] private Inharitance ınh;
     private float tp = 2f;

     private void Start()
     {
          base.Start();
          SetValues();
     }
     protected  override void SetValues()
     {
          base.SetValues();
          item2.patrolSpeed = 100;
          Debug.Log("inharite patrol speed : "+item2.patrolSpeed);
     }

    
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
