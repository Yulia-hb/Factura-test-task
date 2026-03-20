using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeReference] private EnemyConfig _enemyConfig;
    [SerializeReference] private EnemyAnimator _enemyAnimator;

    public EnemyConfig Config => _enemyConfig;
    public EnemyAnimator Animator => _enemyAnimator;
}
