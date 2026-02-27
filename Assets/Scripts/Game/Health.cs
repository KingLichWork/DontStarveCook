using System;
using UnityEngine;

public class Health
{
    private int _healthValue;
    private int _maxHealthValue;
    public int HealthValue => _healthValue;

    public event Action GameOverAction;
    public static event Action<int, int> HealthChangeAction;

    public Health(int maxHealth)
    {
        _maxHealthValue = maxHealth;
    }

    public void ChangeHealth(int value)
    {
        _healthValue = Math.Clamp(_healthValue + value, 0, _maxHealthValue);

        HealthChangeAction?.Invoke(_healthValue, _maxHealthValue);

        if (value <= 0)
            GameOverAction?.Invoke();
    }
}
