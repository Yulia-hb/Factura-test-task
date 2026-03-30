using UnityEngine;
using UnityEngine.UI;
using System;


public class EndGameView : MonoBehaviour
{
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private Button _winRestartButton;
    [SerializeField] private Button _loseRestartButton;

    public event Action OnRestartClicked;

    private void Awake()
    {
        _winRestartButton.onClick.AddListener(() => OnRestartClicked?.Invoke());
        _loseRestartButton.onClick.AddListener(() => OnRestartClicked?.Invoke());
    }
    public void ShowWin()
    {
        _winPanel.SetActive(true);
    }

    public void ShowLose()
    {
        _losePanel.SetActive(true);
    }

    public void HideAll()
    {
        _winPanel.SetActive(false);
        _losePanel.SetActive(false);
    }
}
