
using UnityEngine;

public class EndGamePresenter
{
    private readonly EndGameView _view;
    private readonly GameUIView _gameView;

    public EndGamePresenter(EndGameView view, GameUIView gameView)
    {
        _view = view;
        _gameView = gameView;

        _view.OnRestartClicked += Restart;
    }

    public void Win()
    {
        _gameView.Hide();
        _view.ShowWin();
    }

    public void Lose()
    {
        _gameView.Hide();
        _view.ShowLose();
    }

    private void Restart()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}