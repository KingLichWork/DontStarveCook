using System;
using UnityEngine;

public class GoldGetter : MonoBehaviour
{
    public static event Action GetGoldAction;

    public void GetGold(FoodView food)
    {
        SaveManager.PlayerData.Gold += Mathf.Abs(food.Food.FoodValue + food.Food.HealthValue);
        GetGoldAction.Invoke();
    }
}
