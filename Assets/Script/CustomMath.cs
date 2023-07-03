using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class CustomMath
{
   
  
    public static float AddNumbers(float a ,float b)
    {
        float c = a + b;
        return c;
    }

    public static void VelocityDirectionMovement(Rigidbody rig, Vector3 dir)
    {
        rig.velocity = dir;
    } 
    public static void ChangeVelocity(this Rigidbody rig, Vector3 dir)
    {
        rig.velocity = dir;
    }
    
    
   

   
}
