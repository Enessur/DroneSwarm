using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
 public int health;
 public float chaseSpeed;
 public float patrolSpeed;
 public float damage;
 public  float chasingDistance;
 public float attackDistance;
[SerializeField] private LayerMask whatIsPlayer;

public LayerMask WhatIsPlayerWtf => whatIsPlayer;


 public void SetValue(EnemyScriptableObject copy)
 {
     health = copy.health;
     chaseSpeed = copy.chaseSpeed;
     patrolSpeed = copy.patrolSpeed;
     damage = copy.damage;
     chasingDistance = copy.chasingDistance;
     attackDistance = copy.attackDistance;
     whatIsPlayer = copy.WhatIsPlayerWtf;
 }
 
}
