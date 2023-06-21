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
 public LayerMask whatIsPlayer;

}
