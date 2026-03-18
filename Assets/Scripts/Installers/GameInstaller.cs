using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private CarMovement _carMovement;
    [SerializeField] private InputHandler _inputHandler;

    public override void InstallBindings()
    {
        Container.Bind<CarMovement>().FromInstance(_carMovement).AsSingle();
        Container.Bind<InputHandler>().FromInstance(_inputHandler).AsSingle();

        Container.Bind<GameController>().AsSingle().NonLazy();
    }
}
