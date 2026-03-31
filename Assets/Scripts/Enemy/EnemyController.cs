using System.Collections;
using UnityEngine;
using Zenject;

public class EnemyController : MonoBehaviour
{
    private GameUIPresenter _uiPresenter;
    private GameController _gameController;
    [Inject]
    public void Construct(GameUIPresenter presenter, GameController gameController)
    {
        _uiPresenter = presenter;
        _gameController = gameController;
    }

    [SerializeField] private Enemy _enemy;
    [SerializeField] private Transform _car;

    private State _currentState;
    private Health _health;
    private Rigidbody _rb;

    private bool _isDead;
    private bool _gameStarted;
    private float _attackTimer;

    private Vector3 _offset;
    private Vector3 _moveDirection;
    public event System.Action OnDeathFinished;

    private enum State
    {
        Idle,
        Run,
        Attack
    }

    private void Awake()
    {
        _health = GetComponent<Health>();
        _rb = GetComponent<Rigidbody>(); 
    }

    private void Update()
    {
        if (_gameController.CurrentState != GameState.Playing)
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
            _moveDirection = Vector3.zero;
        }

        else if (distance > _enemy.Config.attackDistance + 0.2f)
        {
            SetState(State.Run);

            Vector3 direction = (_car.position - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);
            }

            float stopDistance = _enemy.Config.attackDistance + 0.3f;
            if (distance < stopDistance)
            {
                _moveDirection = Vector3.zero;
                return;
            }

            _moveDirection = direction;
        }

        else
        {
            SetState(State.Attack);

            _moveDirection = Vector3.zero;

            _attackTimer += Time.deltaTime;

            if (_attackTimer >= _enemy.Config.attackCooldown)
            {
                _attackTimer = 0f;

                AttackHit();
                Debug.Log("HIT");
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_gameStarted || _isDead) return;

        if (_currentState == State.Run && _moveDirection != Vector3.zero)
        {
            _rb.MovePosition(
                _rb.position + _moveDirection * _enemy.Config.moveSpeed * Time.fixedDeltaTime
            );
            _rb.angularVelocity = Vector3.zero;
        }

        Vector3 pos = _rb.position;
        pos.y = _car.position.y;
        _rb.position = pos;
        _rb.angularVelocity = Vector3.zero;
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
        _currentState = (State)(-1);
    }

    public void SetTarget(Transform car)
    {
        _car = car;

        float side = Random.Range(2f, 4f) * (Random.value > 0.5f ? 1 : -1);

        float forward = Random.Range(3f, 6f);

        _offset = new Vector3(
            side,
            0f,
            forward
        );
        
    }

    public void StartGame()
    {
        _gameStarted = true;
        _rb.isKinematic = false;
    }

    private void AttackHit()
    {
        var carHealth = _car.GetComponentInChildren<Health>();

        if (carHealth != null)
        {
            carHealth.TakeDamage(_enemy.Config.damage);
        }
    }

    private void OnDeath()
    {
        if (_isDead) return;

        _isDead = true;

        StopAllCoroutines();
        _attackTimer = 0f;

        foreach (var col in GetComponentsInChildren<Collider>())
            col.enabled = false;

        _enemy.Animator.SetRunning(false);
        _enemy.Animator.SetAttacking(false);

        _enemy.Animator.Die();

        _uiPresenter.AddCoins(100);

        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(2.5f);
        OnDeathFinished?.Invoke();
    }

    public void ResetState()
    {
        _isDead = false;
        _attackTimer = 0f;

        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        foreach (var col in GetComponentsInChildren<Collider>())
            col.enabled = true;

        _health.OnDeath -= OnDeath;
        _health.OnDeath += OnDeath;

        SetState(State.Idle);
    }
}
