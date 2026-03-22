using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Scriptable Objects/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed;

    [Header("AI")]
    public float chaseDistance;
    public float attackDistance;

    [Header("Combat")]
    public float damage;
    public float attackCooldown;

    [Header("Stats")]
    public float maxHealth;
}
