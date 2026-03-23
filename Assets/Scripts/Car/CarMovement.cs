using System;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private bool _isMoving;

    public event Action OnStartMove;
    public void StartMove()
    {
        _isMoving = true;
        OnStartMove?.Invoke();
    }

    private void Update()
    {
        if (!_isMoving) return;

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
