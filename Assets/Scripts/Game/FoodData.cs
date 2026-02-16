using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodData", menuName = "Game/FoodData")]
public class FoodData : ScriptableObject
{
    [SerializeField] private List<Food> _food = new();

    public IReadOnlyList<Food> Food => _food;
}
