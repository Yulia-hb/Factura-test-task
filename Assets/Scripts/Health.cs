using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHP = 100;

    public int CurrentHP { get; private set; }

    public event Action OnDeath;

    private void Awake()
    {
        CurrentHP = _maxHP;
    }

    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;

        if (CurrentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }
}
