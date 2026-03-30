using UnityEngine;

public class GameUIPresenter
{
    private readonly GameUIView _view;
    private readonly RoadLooper _road;

    private readonly Color _startColor = Color.red;
    private readonly Color _endColor = Color.green;

    private float _currentProgress;
    private bool _isPaused;
    public GameUIPresenter(GameUIView view, RoadLooper road)
    {
        _view = view;
        _road = road;

        _view.OnPauseClicked += Pause;
    }

    public void Tick(float deltaTime)
    {
        float target = _road.GetProgress();

        _currentProgress = Mathf.Lerp(_currentProgress, target, deltaTime * 5f);

        _view.SetProgress(_currentProgress);

        Color color = Color.Lerp(Color.red, Color.green, _currentProgress);
        _view.SetProgressColor(color);
    }

    public void AddCoins(int amount)
    {
        _coins += amount;
        _view.SetCoins(_coins);
    }

    private int _coins;

    private void Pause()
    {
        _isPaused = !_isPaused;

        Time.timeScale = _isPaused ? 0f : 1f;
    }
}
