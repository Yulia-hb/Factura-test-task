using Zenject;
using UnityEngine;

public class TurretShootController : IFixedTickable
{
    private readonly TurretShooter _shooter;
    private readonly GameController _gameController;

    private float _shootDelay = 0.2f;
    private float _timer;
    private bool _wasPlaying;

    public TurretShootController(TurretShooter shooter, GameController gameController)
    {
        _shooter = shooter;
        _gameController = gameController;
    }

    public void FixedTick()
    {
        if (_gameController.CurrentState != GameState.Playing)
        {
            _wasPlaying = false;
            return;
        }

        if (!_wasPlaying)
        {
            _timer = 0f;
            _wasPlaying = true;
        }

        _timer += Time.fixedDeltaTime;

        if (_timer >= _shootDelay)
        {
            _timer -= _shootDelay;

            _shooter.Tick();
        }
    }
}
