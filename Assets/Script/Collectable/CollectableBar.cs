using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CollectableBar : MonoBehaviour
{
    [SerializeField] private Image _collectableBarSprite;
    private Camera _cam;

     void Start()
    {
        _cam = Camera.main;
        
    }

    public void UpdateCollectableBar(float maxCollectable, float currentCollectable)
    {
        _collectableBarSprite.fillAmount = currentCollectable / maxCollectable;
    }

     void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    }
}
