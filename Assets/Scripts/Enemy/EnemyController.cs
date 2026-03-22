using System.Collections;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Transform _car;
    private Vector3 _offset;
    private float _attackTimer;

    private Health _health;
    private bool _isDead;
    private float _startY;

    private enum State
    {
        Idle,
        Run,
        Attack
    }

    private State _currentState;

    private void Awake()
    {
          _offset = new Vector3(
          Random.Range(-2f, 2f),
          0f,
         Random.Range(-2f, 2f));
      
        _startY = transform.position.y;
        _health = GetComponent<Health>();
        _health.OnDeath += OnDeath;

        SetState(State.Idle);
   
    }

    private void Update()
    {
        if (_isDead)
            return;

        float distance = Vector3.Distance(transform.position, _car.position);

        if (distance > _enemy.Config.chaseDistance)
        {
            SetState(State.Idle);
        }
        else if (distance > _enemy.Config.attackDistance + 2f)
        {
            SetState(State.Run);

            Vector3 direction = (_car.position - transform.position).normalized;

            // ПОВОРОТ
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);
            }

            // РУХ
            float speedMultiplier = distance < _enemy.Config.attackDistance + 1f ? 0.5f : 1f;

            transform.position += direction * _enemy.Config.moveSpeed * speedMultiplier * Time.deltaTime;

            // 🔥 ФІКС ВИСОТИ
            Vector3 pos = transform.position;
            pos.y = _startY;
            transform.position = pos;
        }
        else
        {
            SetState(State.Attack);
            _attackTimer += Time.deltaTime;

            if (_attackTimer >= _enemy.Config.attackCooldown)
            {
                _attackTimer = 0f;

                AttackHit();
            }
        }
    }

    private void SetState(State newState)
    {
        if (_isDead) return;

        if (_currentState == newState)
            return;

        _currentState = newState;

        switch (_currentState)
        {
            case State.Idle:
                _enemy.Animator.SetRunning(false);
                _enemy.Animator.SetAttacking(false);
                break;

            case State.Run:
                _enemy.Animator.SetRunning(true);
                _enemy.Animator.SetAttacking(false);
                break;

            case State.Attack:
                _enemy.Animator.SetRunning(false);
                _enemy.Animator.SetAttacking(true);
                break;
        }
    }

    private void AttackHit()
    {
        var carHealth = _car.GetComponent<Health>();
        carHealth.TakeDamage(_enemy.Config.damage);
    }

    private void OnDeath()
    {
        if (_isDead) return;

        _isDead = true;

        _enemy.Animator.SetRunning(false);
        _enemy.Animator.SetAttacking(false);
        _enemy.Animator.Die();

        StartCoroutine(DisableAfterDeath());
    }

    private IEnumerator DisableAfterDeath()
    {
        yield return new WaitForSeconds(2f); // під довжину анімації
        gameObject.SetActive(false);
    }
}

