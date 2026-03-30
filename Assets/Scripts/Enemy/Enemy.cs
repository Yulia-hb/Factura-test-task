using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeReference] private EnemyConfig _enemyConfig;
    [SerializeReference] private EnemyAnimator _enemyAnimator;
    public EnemyConfig Config => _enemyConfig;
    public EnemyAnimator Animator => _enemyAnimator;

    public void OnSpawned(Transform car)
    {
        var health = GetComponent<Health>();
        health.ResetHealth();

        var controller = GetComponent<EnemyController>();
        controller.ResetState();
        GetComponent<Rigidbody>().isKinematic = true;
        controller.SetTarget(car);

        foreach (var col in GetComponentsInChildren<Collider>())
            col.enabled = true;

        var rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.angularVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.position = transform.position;
        }
    }
}
   