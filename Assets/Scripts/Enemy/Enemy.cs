using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeReference] private EnemyConfig _enemyConfig;
    [SerializeReference] private EnemyAnimator _enemyAnimator;

    public EnemyConfig Config => _enemyConfig;
    public EnemyAnimator Animator => _enemyAnimator;

    public void OnSpawned(Transform car)
    {
        // Health
        var health = GetComponent<Health>();
        health.ResetHealth();

        // Controller
        var controller = GetComponent<EnemyController>();
        controller.ResetState();
        controller.SetTarget(car);

        // Collider (на всякий)
        foreach (var col in GetComponentsInChildren<Collider>())
            col.enabled = true;
    }
}
