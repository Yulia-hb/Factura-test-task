using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Transform _car;
    [SerializeField] private CarMovement _carMovement;

    [Header("Settings")]
    [SerializeField] private int _startEnemies = 4;
    [SerializeField] private int _maxEnemies = 8;
    [SerializeField] private float _spawnInterval = 2f;

    [Header("Spawn Distances")]
    [SerializeField] private float _spawnForwardMin = 10f;
    [SerializeField] private float _spawnForwardMax = 20f;
    [SerializeField] private float _spawnSideRange = 4f;

    private float _timer;
    private int _currentEnemies;
    private bool _gameStarted;

    private List<EnemyController> _enemies = new();
    private EnemyPool _pool;

    // 🔥 НОВЕ — для росту складності
    private int _spawnCount = 1;
    private float _difficultyTimer;

    [Inject]
    public void Construct(EnemyPool pool)
    {
        _pool = pool;
    }

    private void Start()
    {
        for (int i = 0; i < _startEnemies; i++)
        {
            SpawnEnemy(false);
        }

        _carMovement.OnStartMove += StartGame;
    }

    private void Update()
    {
        if (!_gameStarted)
            return;

        // 🔥 РІСТ КІЛЬКОСТІ ВОРОГІВ
        _difficultyTimer += Time.deltaTime;

        if (_difficultyTimer >= 10f)
        {
            _difficultyTimer = 0f;
            _spawnCount ++; // кожні 10 сек +2 ворог
        }

        _timer += Time.deltaTime;

        if (_timer >= _spawnInterval)
        {
            _timer = 0f;

            if (_currentEnemies < _maxEnemies)
            {
                int count = Mathf.Min(_spawnCount, _maxEnemies - _currentEnemies);

                for (int i = 0; i < count; i++)
                {
                    SpawnEnemy(true);
                }
            }
        }
    }

    private void StartGame()
    {
        _gameStarted = true;

        foreach (var enemy in _enemies)
        {
            if (enemy != null)
                enemy.StartGame();
        }
    }

    private void SpawnEnemy(bool active)
    {
        Vector3 pos = GetSpawnPosition();

        Enemy enemy = _pool.Spawn(pos);

        // 🔥 ВСЕ РЕСЕТИМО В ОДНОМУ МІСЦІ
        enemy.OnSpawned(_car);

        EnemyController controller = enemy.GetComponent<EnemyController>();

        if (active)
        {
            controller.StartGame();
            controller.ForceUpdateState();
        }

        _enemies.Add(controller);
        _currentEnemies++;

        var health = enemy.GetComponent<Health>();

        System.Action deathAction = null;

        deathAction = () =>
        {
            _currentEnemies--;

            _enemies.Remove(controller);

            health.OnDeath -= deathAction;

            _pool.Despawn(enemy);
        };

        health.OnDeath += deathAction;
    }

    private Vector3 GetSpawnPosition()
    {
        Vector3 forward = _car.forward;
        Vector3 right = _car.right;

        float forwardDist = Random.Range(_spawnForwardMin, _spawnForwardMax);
        float side = Random.Range(-_spawnSideRange, _spawnSideRange);

        Vector3 pos = _car.position + forward * forwardDist + right * side;

        pos.y = 0f;

        return pos;
    }
}