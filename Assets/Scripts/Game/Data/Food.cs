using UnityEngine;

[CreateAssetMenu(menuName = "Game/Food")]
public class Food : FoodBase
{
    [SerializeField] private FoodType _type;
    [SerializeField] private float _foodTypeValue;

    [SerializeField] private Food _cookedVersion;

    public FoodType FoodType => _type;
    public float FoodTypeValue => _foodTypeValue;

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

