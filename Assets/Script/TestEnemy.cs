using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Rigidbody _rb;
    public Rigidbody Rb => _rb;
    public List<TestEnemy> enemyTransforms = new();
    private float _size =10f;
    private float _speed =1f;
    
    void Start()
    {
        TargetManager.Instance.AddEnemy(this);
    }
    
    // Update is called once per frame
    void Update()
    {
        // var dir = new Vector3(Mathf.Cos(Time.time * _speed) * _size, Mathf.Sin(Time.time * _speed) * _size);
        // _rb.velocity = dir;

    }
}
