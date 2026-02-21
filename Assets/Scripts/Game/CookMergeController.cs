using System.Collections.Generic;
using UnityEngine;

public class CookMergeController
{
    private RecipesData _recipesData;
    private Recipe _gruel;

    public CookMergeController(RecipesData recipesData, Recipe gruel)
    {
        _recipesData = recipesData;
        _gruel = gruel;
    }

    public FoodBase Cook(Food[] ingredients)
    {
        foreach (var recipe in _recipesData.Recipes)
        {
            if (IsRecipeMatch(recipe, ingredients))
                return recipe;
        }

        return _gruel;
    }

    private bool IsRecipeMatch(Recipe recipe, Food[] ingredients)
    {
        foreach (var ingredient in ingredients)
        {
            foreach (var excluded in recipe.ExcludeFoodTypes)
            {
                if (ingredient.FoodType == excluded)
                    return false;
            }
        }

        Dictionary<FoodType, float> typeValues = new Dictionary<FoodType, float>();

        foreach (var ingredient in ingredients)
        {
            if (!typeValues.ContainsKey(ingredient.FoodType))
                typeValues[ingredient.FoodType] = 0;

            typeValues[ingredient.FoodType] += ingredient.FoodTypeValue;
        }

        foreach (var requirement in recipe.RecipeFoodTypes)
        {
            if (!typeValues.TryGetValue(requirement.FoodType, out float value))
                return false;

            if (value < requirement.ValueFoodType)
                return false;
        }

        return true;
    }
}
