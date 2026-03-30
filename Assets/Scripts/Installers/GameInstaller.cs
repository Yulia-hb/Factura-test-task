using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private CarMovement _carMovement;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private TurretAim _turretAim;
    [SerializeField] private TurretShooter _turretShooter;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private EndGameView _endGameView;
    [SerializeField] private GameUIView _gameUIView;
    [SerializeField] private RoadLooper _roadLooper;

    public override void InstallBindings()
    {      
       
        Container.Bind<CarMovement>().FromComponentInHierarchy().AsSingle();
        Container.Bind<InputHandler>().FromInstance(_inputHandler).AsSingle();

        Container.Bind<GameController>().AsSingle().NonLazy();

        Container.Bind<TurretAim>().FromInstance(_turretAim).AsSingle();
        Container.Bind<TurretShooter>().FromInstance(_turretShooter).AsSingle();

        Container.BindInterfacesTo<TurretAimController>().AsSingle();
        Container.BindInterfacesTo<TurretShootController>().AsSingle();

        Container.Bind<EndGameView>().FromInstance(_endGameView).AsSingle();
        Container.Bind<EndGamePresenter>().AsSingle();

        Container.Bind<GameUIView>().FromInstance(_gameUIView).AsSingle();
        Container.Bind<GameUIPresenter>().AsSingle();
        Container.Bind<RoadLooper>().FromInstance(_roadLooper).AsSingle();

        Container.BindMemoryPool<Bullet, BulletPool>()
            .WithInitialSize(0)
            .FromComponentInNewPrefab(_bulletPrefab)
            .UnderTransformGroup("Bullets");

        Container.BindMemoryPool<Enemy, EnemyPool>()
           .WithInitialSize(10)
           .FromComponentInNewPrefab(_enemyPrefab)
           .UnderTransformGroup("Enemies");
    }
}
