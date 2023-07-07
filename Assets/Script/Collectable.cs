using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] private int _totalAvailable = 200;
    private int _available;
    
    
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   

}
