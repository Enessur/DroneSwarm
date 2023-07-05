using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Collectable> collectables = new();
    private int contains = 100;
    
    void Start()
    {
        TargetManager.Instance.AddCollectable(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GatherObj(int collect)
    {
        contains -= collect;
    }
    

}
