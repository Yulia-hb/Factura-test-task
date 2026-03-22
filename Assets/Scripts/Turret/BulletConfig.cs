using UnityEngine;

[CreateAssetMenu(menuName = "Configs/BulletConfig")]
public class BulletConfig : ScriptableObject
{
    [Header("Movement")]
    public float speed;        // швидкість кулі

    [Header("Combat")]
    public float damage;       // урон

    [Header("Lifetime")]
    public float lifetime;     // через скільки зникає
}
