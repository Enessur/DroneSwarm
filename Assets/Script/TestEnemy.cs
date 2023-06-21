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
      //  TargetManager.Instance.AddEnemy(this);
    }

}
