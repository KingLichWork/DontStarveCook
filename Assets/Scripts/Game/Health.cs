using System;
using UnityEngine;

public class Health
{
    private int _healthValue;
    private int _maxHealthValue;

    public int HealthValue => _healthValue;
    public int MaxHealthValue => _maxHealthValue;

    public static event Action GameOverAction;
    public static event Action<int, int> HealthChangeAction;

    public Health(int currentValue = 100, int maxHealth = 100)
    {
        _healthValue = currentValue;
        _maxHealthValue = maxHealth;
    }

    public void ChangeMaxValue(int value)
    {
        _maxHealthValue += value;
        _healthValue += value;
        HealthChangeAction?.Invoke(_healthValue, _maxHealthValue);
    }

    public void ChangeHealth(int value)
    {
        _healthValue = Math.Clamp(_healthValue + value, 0, _maxHealthValue);

        HealthChangeAction?.Invoke(_healthValue, _maxHealthValue);

        if (_healthValue <= 0)
            GameOverAction?.Invoke();
    }
}
