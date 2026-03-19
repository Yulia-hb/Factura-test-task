using Zenject;

public class TurretAimController : ITickable
{
    private readonly TurretAim _aim;
    private readonly GameController _gameController;

    public TurretAimController(TurretAim aim, GameController gameController)
    {
        _aim = aim;
        _gameController = gameController;
    }

    public void Tick()
    {
        if (_gameController.CurrentState != GameState.Playing)
            return;

        _aim.Tick();
    }
}
