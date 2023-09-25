using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SmoothCam : MonoBehaviour
{
     public CinemachineVirtualCamera virtualCamera;
     public Transform target;
     public MotherShipMovement motherShipMovement;
     private float _speed;
     private CinemachineFramingTransposer _framingTransposer;
     private Vector3 previousPosition;
     private void Start()
     {
          virtualCamera = GetComponent<CinemachineVirtualCamera>();
          _framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
          target = virtualCamera.Follow;
          previousPosition = target.position;
          
     }

     private void Update()
     {
          float dampValue = Mathf.Lerp(5f, 0.5f, Mathf.InverseLerp(0f, motherShipMovement.maxSpeed, motherShipMovement.currentSpeed));
          float track = Mathf.Lerp(7f, 3f, Mathf.InverseLerp(0f, motherShipMovement.maxSpeed, motherShipMovement.currentSpeed));
       
          _framingTransposer.m_XDamping = dampValue;
          _framingTransposer.m_YDamping = dampValue;
          _framingTransposer.m_ZDamping = dampValue;

          _framingTransposer.m_TrackedObjectOffset.z = track;
     }
}
