using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MotherShip : Script.Singleton<MotherShip>
{
    [SerializeField] private float moveSpeed = 60f;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private int _maxHeld =2000;
    [SerializeField] private Text _gatheredText;
    
    
    public float gathered;
    public bool isMotherShipStorageFull;
    public Rigidbody rb;
    public Transform childObject;
    private Quaternion targetRotation;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetRotation = childObject.rotation;
        _gatheredText.text = "Gathered Resources = " + gathered;
    }

    public void AddResource(float addResource)
    {
        gathered += addResource;
        if (gathered >= _maxHeld)
        {
            isMotherShipStorageFull = true;
        }
        else
        {
            isMotherShipStorageFull = false;
        }

        _gatheredText.text = "Gathered Resources = " + gathered;
    }

    public void RemoveResource(float removeResource)
    {
        gathered -= removeResource;

        _gatheredText.text = "Gathered Resources = " + gathered;
    }


    private void FixedUpdate()
    {

        
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(hAxis, 0, vAxis).normalized;  

        Vector3 movement = inputDirection * moveSpeed * Time.deltaTime;

        rb.MovePosition(transform.position + movement);

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement.normalized, Vector3.up);
            childObject.rotation = Quaternion.RotateTowards(childObject.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }
}