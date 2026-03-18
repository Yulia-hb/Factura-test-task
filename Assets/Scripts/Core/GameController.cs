using UnityEngine;

public class GameController
{
    private readonly CarMovement _carMovement;
    private readonly InputHandler _input;

    public GameController(CarMovement carMovement, InputHandler input)
    {
        _carMovement = carMovement;
        _input = input;

        _input.OnTap += StartGame;
    }

    private void StartGame()
    {
        _carMovement.StartMove();
    }
}
