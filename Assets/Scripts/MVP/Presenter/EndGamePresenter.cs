

using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGamePresenter
{
    private readonly EndGameView _view;

    public EndGamePresenter(EndGameView view)
    {
        _view = view;

        _view.OnRestartClicked += Restart;
    }

    public void Win()
    {
        _view.ShowWin();
    }

    public void Lose()
    {
        _view.ShowLose();
    }

    private void Restart()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }
}