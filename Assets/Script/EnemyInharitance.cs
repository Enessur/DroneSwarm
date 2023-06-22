using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInharitance : EnemyBehaviour
{
    public enum Inharitance
    {
        Teleport,
        None
    }

    [SerializeField] private Inharitance ınh = Inharitance.None;
    private float tp = 2f;


    protected override void Start()
    {
        base.Start();
        m_enemyDataInstance.patrolSpeed = 50;
    }

    protected override void Update()
    {
        base.Update();

        if (Vector3.Distance(transform.position, _target.position) < tp)
        {
            ınh = Inharitance.Teleport;
        }
        else
        {
            ınh = Inharitance.None;
        }

        switch (ınh)
        {
            case Inharitance.Teleport:
                Debug.Log("aksjfhasd");
                break;
            case Inharitance.None:
                break;
        }
    }
}