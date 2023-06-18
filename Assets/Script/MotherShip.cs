using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShip : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 60f;
    private Vector3 _moveDir;
    private Rigidbody2D _rb;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleControl();
        _rb.velocity = _moveDir * moveSpeed;
    }

    private void FixedUpdate()
    {
        _rb.velocity = _moveDir * moveSpeed;
    }

    private void HandleControl()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            moveY = +1f;

        }

        if (Input.GetKey(KeyCode.S))
        {
            moveY = -1f;

        }

        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;

        }

        if (Input.GetKey(KeyCode.D))
        {
            moveX = +1f;

        }

        _moveDir = new Vector3(moveX, moveY).normalized;

    }
}
