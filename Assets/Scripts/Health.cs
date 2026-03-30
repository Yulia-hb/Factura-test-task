using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHP = 100;

    public int CurrentHP { get; private set; }
    public event Action OnDeath;
    private bool _isDead;

    private void Awake()
    {
        CurrentHP = _maxHP;
    }

    public void TakeDamage(float damage)
    {
        if (_isDead) return; 

        int dmg = Mathf.RoundToInt(damage);
        CurrentHP -= dmg;

        Debug.Log($"{gameObject.name} took {dmg} damage | HP: {CurrentHP}");       

        if (CurrentHP <= 0)
        {
            Die();
        }
    }

    public void ResetHealth()
    {
        CurrentHP = _maxHP;
        _isDead = false;

    }

    private void Die()
    {
        if (_isDead) return;

        _isDead = true;

        Debug.Log($"{gameObject.name} DIED 💀");

        OnDeath?.Invoke();
    }
}
