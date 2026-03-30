using UnityEngine;

public enum GameState
{
    Waiting,
    Playing,
    Lose,
    Win
}

public class GameController
{
    private readonly CarMovement _carMovement;
    private readonly InputHandler _input;

    public GameState CurrentState { get; private set; } = GameState.Waiting;

    public GameController(CarMovement carMovement, InputHandler input)
    {
        _carMovement = carMovement;
        _input = input;

        _input.OnTap += StartGame;
    }

    private void StartGame()
    {
        if (CurrentState != GameState.Waiting)
            return;

        SetState(GameState.Playing);
    }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;

        switch (CurrentState)
        {
            case GameState.Playing:
                _carMovement.StartMove();
                break;

            case GameState.Win:
            case GameState.Lose:
                _carMovement.StopMove();
                break;
        }
    }
}
