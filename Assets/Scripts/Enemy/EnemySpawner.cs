using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        // стартові вороги
        for (int i = 0; i < _startEnemies; i++)
        {
            SpawnEnemy(false);
        }

        // підписка на старт руху
        _carMovement.OnStartMove += StartGame;
    }

    private void Update()
    {
        if (!_gameStarted)
            return;

        _timer += Time.deltaTime;

        if (_timer >= _spawnInterval)
        {
            _timer = 0f;

            if (_currentEnemies < _maxEnemies)
            {
                SpawnEnemy(true);
            }
        }
    }

    private void StartGame()
    {
        _gameStarted = true;

        // активуємо всіх існуючих ворогів
        foreach (var enemy in _enemies)
        {
            if (enemy != null)
                enemy.StartGame();
        }
    }

    private void SpawnEnemy(bool active)
    {
        Vector3 pos = GetSpawnPosition();

        Enemy enemy = Instantiate(_enemyPrefab, pos, Quaternion.identity);

        EnemyController controller = enemy.GetComponent<EnemyController>();

        controller.SetTarget(_car);

        if (active)
            controller.StartGame(); // нові одразу активні

        _enemies.Add(controller);

        _currentEnemies++;

        enemy.GetComponent<Health>().OnDeath += () =>
        {
            _currentEnemies--;
            _enemies.Remove(controller);
        };
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

