using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMotherShip : MonoBehaviour
{
    
   
    // public MotherShip _motherShip;
    // [SerializeField] private float _rotateSpeed;
    // private Quaternion targetRotation;
    // private Vector3 moveDir;
    // private Vector3 previousPosition;
    //
    // // Start is called before the first frame update
    // void Start()
    // {
    //    
    // }
    //
    // // Update is called once per frame
    // void Update()
    // {
    //     RotateDrone();
    // }
    // private void RotateDrone()
    // {
    //     Vector3 currentVelocity = (transform.position - previousPosition) / Time.deltaTime;
    //     previousPosition = transform.position;
    //
    //     if (currentVelocity != Vector3.zero)
    //     {
    //         Vector3 heading = currentVelocity.normalized;
    //         Quaternion desiredRotation = Quaternion.LookRotation(heading, transform.up);
    //         targetRotation = Quaternion.Slerp(targetRotation, desiredRotation, _rotateSpeed * Time.deltaTime);
    //         _motherShip._rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation,
    //             (_rotateSpeed * 1.5f) * Time.deltaTime));
    //     }
    // }
}
