using System;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
   
    [Header("Feel")]
    [SerializeField] private float swayAmount = 0.2f;
    [SerializeField] private float swaySpeed = 2f;

    [SerializeField] private float shakeAmount = 0.05f;
    [SerializeField] private float shakeSpeed = 3f;
    [SerializeField] private ParticleSystem _smoke;
    [SerializeField] private float speed = 5f;

    private bool _isMoving;
    private float _baseX;
    private float _time;

    private Rigidbody _rb; 
    public event Action OnStartMove;

    private void Start()
    {
        _baseX = transform.position.x;
        _rb = GetComponent<Rigidbody>();
        
    }

    public void StartMove()
    {
        _isMoving = true;
        OnStartMove?.Invoke();
        _smoke.Play();
        
    }

    public void StopMove()
    {
        _isMoving = false;
        _smoke.Stop();
    }

    private void FixedUpdate()
    {
        if (!_isMoving) return;

        _time += Time.fixedDeltaTime; 
      
        Vector3 forwardMove = Vector3.forward * speed * Time.fixedDeltaTime;

        float sway = Mathf.Sin(_time * swaySpeed) * swayAmount;

        float shake = Mathf.Sin(_time * shakeSpeed) * shakeAmount;

        Vector3 targetPos = _rb.position;

        targetPos += forwardMove;
        targetPos.x = _baseX + sway + shake;

        _rb.MovePosition(targetPos); 
    }
}