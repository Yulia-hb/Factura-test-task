using UnityEngine;
using Zenject;

public class CarDeathHandler : MonoBehaviour
{
    private Health _health;
    private EndGamePresenter _presenter;

    [Inject]
    public void Construct(EndGamePresenter presenter)
    {
        _presenter = presenter;
    }

    private void Awake()
    {
        _health = GetComponent<Health>();
        _health.OnDeath += OnCarDeath;
    }

    private void OnCarDeath()
    {
        Debug.Log("CAR DEAD → LOSE");

        _presenter.Lose();

        Time.timeScale = 0f;
    }
}
