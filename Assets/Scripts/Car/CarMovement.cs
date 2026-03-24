using System;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    [Header("Feel")]
    [SerializeField] private float swayAmount = 0.2f;   // вліво-вправо
    [SerializeField] private float swaySpeed = 2f;

    [SerializeField] private float shakeAmount = 0.05f; // тряска
    [SerializeField] private float shakeSpeed = 10f;

    private bool _isMoving;

    private float _baseX;
    private float _time;

    public event Action OnStartMove;

    private void Start()
    {
        _baseX = transform.position.x;
    }

    public void StartMove()
    {
        _isMoving = true;
        OnStartMove?.Invoke();
    }

    public void StopMove()
    {
        _isMoving = false;
    }

    private void Update()
    {
        if (!_isMoving) return;

        _time += Time.deltaTime;

        // 🔥 рух вперед
        Vector3 forwardMove = Vector3.forward * speed * Time.deltaTime;

        // 🔥 sway (вліво-вправо)
        float sway = Mathf.Sin(_time * swaySpeed) * swayAmount;

        // 🔥 shake (дрібна тряска)
        float shake = Mathf.Sin(_time * shakeSpeed) * shakeAmount;

        Vector3 pos = transform.position;

        pos += forwardMove;

        pos.x = _baseX + sway + shake;

        transform.position = pos;
    }
}