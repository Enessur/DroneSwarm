using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    
    [SerializeField] private int _totalAvailable = 200;
    private int _available;
    private Rigidbody _rb;
    public Rigidbody Rb => _rb;


    
    private void OnEnable()
    {
        _available = _totalAvailable;
    }
    
    
    public bool Take()
    {
        if (_available <= 0)
            return false;
        _available--;

        UpdateSize();
        
        return true;
    }

    private void UpdateSize()
    {
        float scale = (float)_available / _totalAvailable;
        if (scale > 0 && scale < 1f)
        {
            var vectorScale = Vector3.one * scale;
            transform.localScale = vectorScale;
        }
        else if (scale <= 0)
        {
            gameObject.SetActive(false);
            TargetManager.Instance.RemoveCollectable(this);
        }

    }

    void Start()
    {
        TargetManager.Instance.AddCollectable(this);
        _rb = GetComponent<Rigidbody>();
    }


}
