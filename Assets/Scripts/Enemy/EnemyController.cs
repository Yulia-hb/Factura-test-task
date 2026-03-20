using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Transform _car;

    private Health _health;
    private bool _isDead;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _health.OnDeath += OnDeath;
    }

    private void Update()
    {
        if (_isDead)
            return;

        float distance = Vector3.Distance(transform.position, _car.position);

        if (distance > _enemy.Config.attackDistance * 2)
        {
            Idle();
        }
        else if (distance > _enemy.Config.attackDistance)
        {
            Run();
        }
        else
        {
            Attack();
        }
    }

    private void Idle()
    {
        _enemy.Animator.SetRunning(false);
        _enemy.Animator.SetAttacking(false);
    }

    private void Run()
    {
        _enemy.Animator.SetRunning(true);
        _enemy.Animator.SetAttacking(false);

        Vector3 direction = (_car.position - transform.position).normalized;
        transform.position += direction * _enemy.Config.moveSpeed * Time.deltaTime;
    }

    private void Attack()
    {
        _enemy.Animator.SetRunning(false);
        _enemy.Animator.SetAttacking(true);
    }

    private void OnDeath()
    {
        _isDead = true;
        _enemy.Animator.Die();
    }
}

