using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int _maxHealth = 3;
    private int _currentHealth;
    
    public HealthUI _healthUI;

    public static event Action OnPlayerDied;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetHealth();

        GameController.OnReset += ResetHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy)
        {
            TakeDamage(enemy.damage);
        }
    }

    private void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _healthUI.UpdateHearts(_currentHealth);

        if (_currentHealth <= 0)
        {
            //die
            OnPlayerDied.Invoke();
        }
    }

    void ResetHealth()
    {
        _currentHealth = _maxHealth;
        _healthUI.SetMaxHearts(_maxHealth);
    }
}
