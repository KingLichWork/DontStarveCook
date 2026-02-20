using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Recipe")]
public class Recipe : ScriptableObject
{
    [SerializeField] private int _foodValue;
    [SerializeField] private int _decayTime;
    [SerializeField] private string _name;
    [SerializeField] private Sprite _sprite;

    [SerializeField] private RecipeFoodType[] _recipeFoodTypes;

    public int FoodValue => _foodValue;
    public int DecayTime => _decayTime;
    public string Name => _name;
    public Sprite Sprite => _sprite;
}

[Serializable]
public class RecipeFoodType
{
    [SerializeField] private float _valueFoodType;
    [SerializeField] private FoodType _foodType;

    public float ValueFoodType => _valueFoodType;

    public FoodType FoodType => _foodType;
}
