using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipesData", menuName = "Game/RecipesData")]
public class RecipesData : ScriptableObject
{
    [SerializeField] private List<Recipe> _recipes = new();

    public IReadOnlyList<Recipe> Recipes => _recipes;
}
