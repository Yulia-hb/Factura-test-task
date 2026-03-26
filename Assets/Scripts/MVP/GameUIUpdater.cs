using UnityEngine;
using Zenject;

public class GameUIUpdater : MonoBehaviour
{
    private GameUIPresenter _presenter;

    [Inject]
    public void Construct(GameUIPresenter presenter)
    {
        _presenter = presenter;
    }

    private void Update()
    {
        _presenter.Tick(Time.deltaTime);
    }
}
