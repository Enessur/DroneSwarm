using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class MotherShipMovement : MonoBehaviour
{
    public enum MovementMode
    {
        Velocity,
        Force
    }
    [TitleGroup("ABC")]
    [SerializeField] private MovementMode movementMode;
    [SerializeField] private bool ısForceMovementOn;
    
    [BoxGroup("Variables")]
    [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
    [SerializeField] private float rotateSpeed = 100f;
    [BoxGroup("Variables")]
    [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
    public float speed;
    [BoxGroup("Variables")]
    [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
    public float currentSpeed;
    [BoxGroup("Variables")]
    [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
    public float maxSpeed;
    [BoxGroup("Scripts")]
    [GUIColor(1f, 0f, 0f, 1f)]
    public VariableJoystick variableJoystick;
    [BoxGroup("Transforms")]
    [GUIColor(1f, 0.8f, 0.2f, 1f)]
    public Transform objectRotation;
    
    
    public Rigidbody rb;
    private Transform control;
    private Vector3 direction;
    private Quaternion objectQuaternion;

    
    public List<Vector3> controlPositionsList;
    private void Start()
    {
        objectQuaternion = objectRotation.rotation;
       
    }

   

    public void FixedUpdate()
    {
        direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        if (ısForceMovementOn == true)
        {
            movementMode = MovementMode.Force;
        }
        else
        {
            movementMode = MovementMode.Velocity;
        }

        switch (movementMode)
        {
            case  MovementMode.Force :
                rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
              
                // Debug.Log(rb.velocity.magnitude);
                if (rb.velocity.magnitude > maxSpeed)
                {
                    rb.velocity = rb.velocity.normalized * maxSpeed;

                }
                break;
            case  MovementMode.Velocity :
                rb.velocity = direction * speed* 100f * Time.deltaTime;
                break;
        }

     
        
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            objectRotation.rotation = Quaternion.RotateTowards(objectRotation.rotation, targetRotation,
                rotateSpeed * Time.fixedDeltaTime);
            
            
        }
        else
        {
            rb.velocity = (direction * speed) / Time.deltaTime;
        }
        currentSpeed = rb.velocity.magnitude;
        
    }
 
}