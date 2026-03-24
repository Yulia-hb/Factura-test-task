using System.Collections;
using UnityEngine;
using Zenject;


public class EnemyController : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Transform _car;
    private State _currentState;
    private Health _health;

    private bool _isDead;
    private bool _gameStarted;

    private float _angle;
    private float _radius;
    private float _attackTimer;
    private Vector3 _offset;

    private enum State
    {
        Idle,
        Run,
        Attack
    }

    private void Awake()
    {
        _health = GetComponent<Health>();           
    }

    private void Update()
    {
        if (!_gameStarted)
        {
            SetState(State.Idle);
            return;
        }

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

            Vector3 offset = new Vector3(
            Mathf.Cos(_angle * Mathf.Deg2Rad),
            0f,
            Mathf.Sin(_angle * Mathf.Deg2Rad)
            ) * _radius;

            Vector3 target = _car.position + offset;
            Vector3 direction = (target - transform.position).normalized;

            // ПОВОРОТ
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);
            }

            // РУХ
            float speedMultiplier = distance < _enemy.Config.attackDistance + 1f ? 0.5f : 1f;

            transform.position += direction * _enemy.Config.moveSpeed * speedMultiplier * Time.deltaTime;

            // 🔥 АНТИ-СТОЛК (ОСЬ ТУТ)
            Collider[] hits = Physics.OverlapSphere(transform.position, 1f);

            foreach (var hit in hits)
            {
                if (hit.gameObject != gameObject && hit.GetComponent<EnemyController>())
                {
                    Vector3 push = (transform.position - hit.transform.position).normalized;
                    transform.position += push * 0.1f * Time.deltaTime;
                }
            }

            // ФІКС ВИСОТИ
            Vector3 pos = transform.position;
            pos.y = _car.position.y;
            transform.position = pos;
        }
        else
        {
            Vector3 toCar = (_car.position - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, toCar);

            if (dot < 0.7f)
            {
                // 🔥 НЕ дивиться → повертається
                Quaternion lookRotation = Quaternion.LookRotation(toCar);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);

                SetState(State.Run);
                return;
            }

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

    public void ForceUpdateState()
    {
        _currentState = (State)(-1); // скидаємо стан
    }

    public void SetTarget(Transform car)
    {
        _car = car;
      
        _offset = new Vector3(
            Random.Range(-3f, 3f),
            0f,
            Random.Range(-3f, 3f)
        );
    }

    public void StartGame()
    {
        _gameStarted = true;
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
        //StartCoroutine(DisableAfterDeath());
    }

    public void ResetState()
    {
        _isDead = false;
        _attackTimer = 0f;

        // 🔥 відписка (на всякий)
        _health.OnDeath -= OnDeath;

        // 🔥 нова підписка
        _health.OnDeath += OnDeath;

        SetState(State.Idle);

    }

    private IEnumerator DisableAfterDeath()
    {
        yield return new WaitForSeconds(2f); // під довжину анімації
        gameObject.SetActive(false);
    }

   
}
