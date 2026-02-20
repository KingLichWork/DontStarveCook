using UnityEngine;

[CreateAssetMenu(menuName = "Game/Food")]
public class Food : ScriptableObject
{
    [SerializeField] private int _foodValue;
    [SerializeField] private int _decayTime;
    [SerializeField] private string _name;
    [SerializeField] private FoodType _type;
    [SerializeField] private float _foodTypeValue;
    [SerializeField] private Sprite _sprite;

    [SerializeField] private Food _cookedVersion;

    public int FoodValue => _foodValue;
    public int DecayTime => _decayTime;
    public string Name => _name;
    public FoodType Type => _type;
    public Sprite Sprite => _sprite;

    public Food CookedVersion => _cookedVersion;

    public bool HasCookedVersion => _cookedVersion != null;
}


public enum FoodType
{
    meat,
    fish,
    vegetable,
    fruit,
    egg,
    monster,
    honey
}

