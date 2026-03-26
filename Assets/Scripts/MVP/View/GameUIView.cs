using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameUIView : MonoBehaviour
{
    [SerializeField] private Button _pauseButton;
    [SerializeField] private TMP_Text _coinsText;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private Image _progressFill;

    public event Action OnPauseClicked;

    private void Awake()
    {
        _pauseButton.onClick.AddListener(() => OnPauseClicked?.Invoke());
    }

    public void SetCoins(int value)
    {
        _coinsText.text = value.ToString();
    }

    public void SetProgress(float value)
    {
        _progressSlider.value = value;
    }

    public void SetProgressColor(Color color)
    {
        _progressFill.color = color;
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
