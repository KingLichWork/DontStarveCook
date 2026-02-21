using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Recipe")]
public class Recipe : FoodBase
{
    [SerializeField] private RecipeFoodType[] _recipeFoodTypes;
    [SerializeField] private FoodType[] _excludeFoodTypes;

    public RecipeFoodType[] RecipeFoodTypes => _recipeFoodTypes;
    public FoodType[] ExcludeFoodTypes => _excludeFoodTypes;
}

[Serializable]
public class RecipeFoodType
{
    [SerializeField] private float _valueFoodType;
    [SerializeField] private FoodType _foodType;

    public float ValueFoodType => _valueFoodType;

    public FoodType FoodType => _foodType;
}
