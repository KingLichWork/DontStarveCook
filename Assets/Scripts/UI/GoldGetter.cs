using System;
using UnityEngine;

public class GoldGetter : MonoBehaviour
{
    public void GetGold(FoodView food)
    {
        ResourcesWallet.AddResource(ResourcesType.Gold, Mathf.Abs(food.Food.FoodValue + food.Food.HealthValue));
    }
}
