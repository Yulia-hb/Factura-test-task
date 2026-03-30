
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.Asteroids;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Transform _car;
    [SerializeField] private CarMovement _carMovement;

    [Header("Level")]
    [SerializeField] private float _levelLength = 375f;
    [SerializeField] private int _maxTotalEnemies = 80;

    [Header("Spawn")]
    [SerializeField] private int _startEnemies = 4;
    [SerializeField] private float _spawnInterval = 2f;

    [Header("Spawn Distances")]
    [SerializeField] private float _spawnForwardMin = 15f;
    [SerializeField] private float _spawnForwardMax = 25f;
    [SerializeField] private float _spawnSideRange = 4f;

    private float _timer;
    private bool _gameStarted;

    private int _currentEnemies;
    private int _spawnedTotal;

    private List<EnemyController> _enemies = new();
    private EnemyPool _pool;
    private GameController _gameController;

    [Inject]
    public void Construct(EnemyPool pool,GameController gameController)
    {
        _pool = pool;
        _gameController = gameController;
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
        if (_gameController.CurrentState != GameState.Playing)
            return;

        float progress = _car.position.z / _levelLength;

        if (progress > 0.9f)
            return;

        _timer += Time.deltaTime;

        if (_timer < _spawnInterval)
            return;

        _timer = 0f;

        if (_spawnedTotal >= _maxTotalEnemies)
            return;

        int count = GetSpawnCountByProgress(progress);

        for (int i = 0; i < count; i++)
        {
            SpawnEnemy(true);
            _spawnedTotal++;
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

    private int GetSpawnCountByProgress(float progress)
    {
        if (progress < 0.3f)
            return 1;

        if (progress < 0.6f)
            return 2;

        if (progress < 0.85f)
            return 3;

        return 1;
    }

    private void SpawnEnemy(bool active)
    {
        Vector3 pos = GetSpawnPosition();

        Enemy enemy = _pool.Spawn(pos);

        enemy.OnSpawned(_car);

        EnemyController controller = enemy.GetComponent<EnemyController>();

        System.Action onDeath = null;

        onDeath = () =>
        {
            controller.OnDeathFinished -= onDeath;

            _currentEnemies--;

            _enemies.Remove(controller);

            if (_pool != null)
                _pool.Despawn(enemy);

            
        };

        controller.OnDeathFinished += onDeath;

        _enemies.Add(controller);
        _currentEnemies++;

        if (active)
        {
            controller.StartGame();
            controller.ForceUpdateState();
        }
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

