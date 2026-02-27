using UnityEngine;

public abstract class FoodBase : ScriptableObject
{
    [SerializeField] protected int _foodValue;
    [SerializeField] protected int _healthValue;

    [SerializeField] protected int _decayTime;
    [SerializeField] protected string _name;
    [SerializeField] protected Sprite _sprite;

    public int FoodValue => _foodValue;
    public int HealthValue => _healthValue;
    public int DecayTime => _decayTime;
    public string Name => _name;
    public Sprite Sprite => _sprite;
}
