using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShip : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 60f;
    private Vector3 _moveDir;
    private Rigidbody _rb;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
      
      
    }

    private void FixedUpdate()
    {
        HandleControl();
    }

    private void HandleControl()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(hAxis, 0, vAxis) * moveSpeed * Time.deltaTime;
        _rb.MovePosition(transform.position+movement);

    }
}
