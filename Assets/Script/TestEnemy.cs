using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TargetManager.Instance.AddEnemy(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
