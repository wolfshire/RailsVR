using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public delegate void OnHealthChange(int newHealth);
    public event OnHealthChange HealthChange;

    public delegate void OnTakeDamage();
    public event OnTakeDamage Damaged;

    public delegate void OnDeath();
    public event OnDeath Death;

    public int startingHealth = 3;
    private int _health;

    void Start()
    {
        _health = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0 || _health <= 0) return;

        _health -= damage;

        if (Damaged != null)
            Damaged();

        HealthChanged();

        if (_health <= 0)
        {
            _health = 0;

            if (Death != null)
                Death();
        }
    }

    public void Heal(int amount)
    {
        if (amount < 0) return;

        _health += amount;

        HealthChanged();
    }

    private void HealthChanged()
    {
        if (HealthChange != null)
            HealthChange(_health);
    }
}
