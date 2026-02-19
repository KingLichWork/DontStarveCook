using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodData", menuName = "Game/FoodData")]
public class FoodData : ScriptableObject
{
    [SerializeField] private List<Food> _foods = new();

    public IReadOnlyList<Food> Foods  => _foods;

    public Food GetRandomFoodByType(FoodType type)
    {
        List<Food> foodsOfType = _foods.Where(f => f.Type == type).ToList();

        return foodsOfType[Random.Range(0, foodsOfType.Count)];
    }

    public Food GetRandomFood()
    {
        return _foods[Random.Range(0, _foods.Count)];
    }
}
