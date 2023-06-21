using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/NewEnemyType")]
public class NewEnemyTypeScriptable : ScriptableObject
{
    public int health;
    public float chaseSpeed;
    public float patrolSpeed;
    public float damage;
    public  float chasingDistance;
    public float attackDistance;
    public LayerMask whatIsPlayer;
}
