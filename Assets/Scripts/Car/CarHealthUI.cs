using UnityEngine;
using UnityEngine.UI;

public class CarHealthUI : MonoBehaviour
{
    [SerializeField] private Health _carHealth;
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _fillImage;

    [Header("Animation")]
    [SerializeField] private float _smoothSpeed = 5f;

    [Header("Colors")]
    [SerializeField] private Color _fullHPColor = Color.green;
    [SerializeField] private Color _lowHPColor = Color.red;

    private float _targetValue;

    private void Start()
    {
        int maxHP = _carHealth.CurrentHP;

        _slider.maxValue = maxHP;
        _slider.value = maxHP;

        _targetValue = maxHP;
    }

    private void Update()
    {
        // 🔥 оновлюємо цільове значення
        _targetValue = _carHealth.CurrentHP;

        // 🔥 плавне зменшення
        _slider.value = Mathf.Lerp(_slider.value, _targetValue, Time.deltaTime * _smoothSpeed);

        // 🔥 нормалізоване HP (0-1)
        float normalized = _slider.value / _slider.maxValue;

        // 🔥 колір від зеленого до червоного
        _fillImage.color = Color.Lerp(_lowHPColor, _fullHPColor, normalized);
    }
}