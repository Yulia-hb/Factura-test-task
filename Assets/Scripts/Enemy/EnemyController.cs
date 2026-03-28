using System.Collections;
using UnityEngine;
using Zenject;

public class EnemyController : MonoBehaviour
{
    private GameUIPresenter _uiPresenter;

    [Inject]
    public void Construct(GameUIPresenter presenter)
    {
        _uiPresenter = presenter;
    }

    public event System.Action OnDeathFinished;

    [SerializeField] private Enemy _enemy;
    [SerializeField] private Transform _car;

    private State _currentState;
    private Health _health;
    private Rigidbody _rb; // 🔥 ДОДАЛИ

    private bool _isDead;
    private bool _gameStarted;

    private float _attackTimer;
    private Vector3 _offset;

    private Vector3 _moveDirection; // 🔥 ДОДАЛИ

    private enum State
    {
        Idle,
        Run,
        Attack
    }

    private void Awake()
    {
        _health = GetComponent<Health>();
        _rb = GetComponent<Rigidbody>(); // 🔥 ДОДАЛИ
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

        // 🔥 ДАЛЕКО — стоїть
        if (distance > _enemy.Config.chaseDistance)
        {
            SetState(State.Idle);
            _moveDirection = Vector3.zero;
        }
        // 🔥 БІЖИТЬ ДО МАШИНИ
        else if (distance > _enemy.Config.attackDistance + 0.2f)
        {
            SetState(State.Run);

            Vector3 target = _car.position + _offset;
            Vector3 direction = (_car.position - transform.position).normalized;

            // ПОВОРОТ (НЕ ЧІПАЄМО)
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);
            }

            // 🔥 НЕ ЛІЗЕ В МАШИНУ
            float stopDistance = _enemy.Config.attackDistance + 0.3f;
            if (distance < stopDistance)
            {
                _moveDirection = Vector3.zero;
                return;
            }

            // 🔥 ЗАПАМʼЯТОВУЄМО НАПРЯМОК (замість transform.position)
            _moveDirection = direction;
        }
        // 🔥 АТАКА
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

        // 🔥 фікс висоти (анти-польоти)
        Vector3 pos = _rb.position;
        pos.y = _car.position.y;
        _rb.position = pos;

        // 🔥 гасимо зайві оберти
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

        // 🔥 вибираємо сторону (ліва або права)
        float side = Random.Range(2f, 4f) * (Random.value > 0.5f ? 1 : -1);

        // 🔥 трохи вперед від машини
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

        // 🔥 СКИДАЄМО ФІЗИКУ (ОСНОВНИЙ ФІКС)
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        foreach (var col in GetComponentsInChildren<Collider>())
            col.enabled = true;

        _health.OnDeath -= OnDeath;
        _health.OnDeath += OnDeath;

        SetState(State.Idle);
    }
}
//using System.Collections;
//using UnityEngine;
//using Zenject;

//public class EnemyController : MonoBehaviour
//{
//    private GameUIPresenter _uiPresenter;

//    [Inject]
//    public void Construct(GameUIPresenter presenter)
//    {
//        _uiPresenter = presenter;
//    }

//    public event System.Action OnDeathFinished;

//    [SerializeField] private Enemy _enemy;
//    [SerializeField] private Transform _car;

//    private State _currentState;
//    private Health _health;

//    private bool _isDead;
//    private bool _gameStarted;

//    private float _attackTimer;
//    private Vector3 _offset;

//    private enum State
//    {
//        Idle,
//        Run,
//        Attack
//    }

//    private void Awake()
//    {
//        _health = GetComponent<Health>();
//    }

//    private void Update()
//    {
//        if (!_gameStarted)
//        {
//            SetState(State.Idle);
//            return;
//        }

//        if (_isDead)
//            return;

//        float distance = Vector3.Distance(transform.position, _car.position);

//        // 🔥 ДАЛЕКО — стоїть
//        if (distance > _enemy.Config.chaseDistance)
//        {
//            SetState(State.Idle);
//        }
//        // 🔥 БІЖИТЬ ДО МАШИНИ
//        else if (distance > _enemy.Config.attackDistance + 0.2f)
//        {
//            SetState(State.Run);

//            Vector3 direction = (_car.position - transform.position).normalized;

//            // ПОВОРОТ
//            if (direction != Vector3.zero)
//            {
//                Quaternion lookRotation = Quaternion.LookRotation(direction);
//                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);
//            }

//            // 🔥 НЕ ЛІЗЕ В МАШИНУ
//            float stopDistance = _enemy.Config.attackDistance + 0.3f;
//            if (distance < stopDistance)
//                return;

//            // РУХ
//            transform.position += direction * _enemy.Config.moveSpeed * Time.deltaTime;

//            // АНТИ-СТОЛК
//            Collider[] hits = Physics.OverlapSphere(transform.position, 1f);

//            foreach (var hit in hits)
//            {
//                if (hit.gameObject != gameObject && hit.GetComponent<EnemyController>())
//                {
//                    Vector3 push = (transform.position - hit.transform.position).normalized;
//                    transform.position += push * 0.1f * Time.deltaTime;
//                }
//            }

//            // ФІКС ВИСОТИ
//            Vector3 pos = transform.position;
//            pos.y = _car.position.y;
//            transform.position = pos;
//        }
//        // 🔥 АТАКА
//        else
//        {
//            SetState(State.Attack);

//            _attackTimer += Time.deltaTime;

//            if (_attackTimer >= _enemy.Config.attackCooldown)
//            {
//                _attackTimer = 0f;

//                AttackHit();
//                Debug.Log("HIT"); // 🔥 перевірка
//            }
//        }
//    }

//    private void SetState(State newState)
//    {
//        if (_isDead) return;

//        if (_currentState == newState)
//            return;

//        _currentState = newState;

//        switch (_currentState)
//        {
//            case State.Idle:
//                _enemy.Animator.SetRunning(false);
//                _enemy.Animator.SetAttacking(false);
//                break;

//            case State.Run:
//                _enemy.Animator.SetRunning(true);
//                _enemy.Animator.SetAttacking(false);
//                break;

//            case State.Attack:
//                _enemy.Animator.SetRunning(false);
//                _enemy.Animator.SetAttacking(true);
//                break;
//        }
//    }

//    public void ForceUpdateState()
//    {
//        _currentState = (State)(-1);
//    }

//    public void SetTarget(Transform car)
//    {
//        _car = car;

//        _offset = new Vector3(
//            Random.Range(-3f, 3f),
//            0f,
//            Random.Range(1.5f, 3f)
//        );
//    }

//    public void StartGame()
//    {
//        _gameStarted = true;
//    }

//    private void AttackHit()
//    {
//        var carHealth = _car.GetComponentInChildren<Health>();

//        if (carHealth != null)
//        {
//            carHealth.TakeDamage(_enemy.Config.damage);
//        }
//    }

//    private void OnDeath()
//    {
//        if (_isDead) return;

//        _isDead = true;

//        StopAllCoroutines();
//        _attackTimer = 0f;

//        // 🔥 вимикаємо колайдери
//        foreach (var col in GetComponentsInChildren<Collider>())
//            col.enabled = false;

//        _enemy.Animator.SetRunning(false);
//        _enemy.Animator.SetAttacking(false);

//        _enemy.Animator.Die();

//        _uiPresenter.AddCoins(100);

//        StartCoroutine(DeathRoutine());
//    }

//    private IEnumerator DeathRoutine()
//    {
//        yield return new WaitForSeconds(2.5f);

//        OnDeathFinished?.Invoke();
//    }

//    public void ResetState()
//    {
//        _isDead = false;
//        _attackTimer = 0f;

//        // 🔥 включаємо колайдери назад
//        foreach (var col in GetComponentsInChildren<Collider>())
//            col.enabled = true;

//        _health.OnDeath -= OnDeath;
//        _health.OnDeath += OnDeath;

//        SetState(State.Idle);
//    }
//}