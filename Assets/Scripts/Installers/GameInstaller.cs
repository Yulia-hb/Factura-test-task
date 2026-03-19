using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private CarMovement _carMovement;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private TurretAim _turretAim;
    [SerializeField] private TurretShooter _turretShooter;

    public override void InstallBindings()
    {      
        Container.Bind<CarMovement>().FromInstance(_carMovement).AsSingle();
        Container.Bind<InputHandler>().FromInstance(_inputHandler).AsSingle();

        Container.Bind<GameController>().AsSingle().NonLazy();

        Container.Bind<TurretAim>().FromInstance(_turretAim).AsSingle();
        Container.Bind<TurretShooter>().FromInstance(_turretShooter).AsSingle();

        Container.BindInterfacesTo<TurretAimController>().AsSingle();
        Container.BindInterfacesTo<TurretShootController>().AsSingle();

        Container.BindMemoryPool<Bullet, BulletPool>()
            .WithInitialSize(0)
            .FromComponentInNewPrefab(_bulletPrefab)
            .UnderTransformGroup("Bullets");
    }
}
