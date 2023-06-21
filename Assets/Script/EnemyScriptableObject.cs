using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
 protected int health;
 protected float chaseSpeed;
 protected float patrolSpeed;
 protected float damage;
 protected  float chasingDistance;
 protected float attackDistance;
 public LayerMask whatIsPlayer;

 public int GetHealth()
 {
  return health;
 }

 public float GetChaseSpeed()
 {
  return chaseSpeed;
 }

 public float GetPatrolSpeed()
 {
  return patrolSpeed;
 }
 public float GetDamage()
 {
  return damage;
 }
 public float GetChasingDistance()
 {
  return chasingDistance;
 }
 public float GetAttackDistance()
 {
  return attackDistance;
 }


}
