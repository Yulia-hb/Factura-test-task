using UnityEngine;

public class GameUIPresenter
{
    private readonly GameUIView _view;
    private readonly RoadLooper _road;

    private float _currentProgress;

    private readonly Color _startColor = Color.red;
    private readonly Color _endColor = Color.green;

    public GameUIPresenter(GameUIView view, RoadLooper road)
    {
        _view = view;
        _road = road;

        _view.OnPauseClicked += Pause;
    }

    public void Tick(float deltaTime)
    {
        float target = _road.GetProgress();

        // 🔥 плавність
        _currentProgress = Mathf.Lerp(_currentProgress, target, deltaTime * 5f);

        _view.SetProgress(_currentProgress);

        // 🔥 колір
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
        Time.timeScale = 0f;
    }
}
