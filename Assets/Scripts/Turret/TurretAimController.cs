using UnityEngine;
using Zenject;

public class TurretAimController : ITickable
{
    private readonly TurretAim _turretAim;
    private readonly GameController _gameController;
    private Transform _target;
    private float _rotateSpeed;

    public TurretAimController(TurretAim aim, GameController gameController)
    {
        _turretAim = aim;
        _gameController = gameController;
    }

    public void Tick()
    {
        if (_gameController.CurrentState != GameState.Playing)
            return;

        _turretAim.Tick();
        RotateToTarget();
    }

    private void RotateToTarget()
    {
       
        if (_turretAim == null)
            return;

        Vector3 direction = _turretAim.transform.forward;

        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        _turretAim.transform.rotation = Quaternion.Slerp(
            _turretAim.transform.rotation,
            targetRotation,
            5f * Time.deltaTime
        );
    }
   
}

