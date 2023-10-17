using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using Unity.VisualScripting;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private float _totalAvailable = 200;
    [SerializeField] private CollectableBar _collectableBar;
    [SerializeField] private Transform _canvasHolder;
    [SerializeField] private GameObject _canvas;


    private float _available;
    private Rigidbody _rb;
    public Rigidbody Rb => _rb;


    void Start()
    {
        TargetManager.Instance.AddCollectable(this);
        _rb = GetComponent<Rigidbody>();
        _collectableBar.UpdateCollectableBar(_totalAvailable, _available);
        _canvasHolder.SetParent(null);
    }

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
        _collectableBar.UpdateCollectableBar(_totalAvailable, _available);

        float scale = (float)_available / _totalAvailable;
        if (scale > 0 && scale < 1f)
        {
            var vectorScale = Vector3.one * scale;
            transform.localScale = vectorScale;
        }
        else if (scale <= 0)
        {
            gameObject.SetActive(false);
            _canvas.SetActive(false);
            TargetManager.Instance.RemoveCollectable(this);
        }
    }
}