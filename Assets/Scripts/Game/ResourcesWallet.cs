using System;
using System.Collections.Generic;
using UnityEngine;
    
public static class ResourcesWallet
{
    private static readonly Dictionary<ResourcesType, Func<int>> _getResources = new()
    {
        { ResourcesType.Gold, () => SaveManager.PlayerData.Gold},
    };

    private static readonly Dictionary<ResourcesType, Action<int>> _setResources = new()
    {
        { ResourcesType.Gold, amount => SaveManager.PlayerData.Gold = amount },
    };

    public static event Action<ResourcesType, int> OnResourcesCountChanged;

    public static bool SpendResource(ResourcesType resType, int amount, Action onError = null)
    {
        int currentAmount = _getResources[resType]();

        if (currentAmount - amount < 0)
        {
            Debug.LogWarning($"You do not have enough {resType.ToString()} to buy this offer.");
            onError?.Invoke();
            return false;
        }
        else
        {
            _setResources[resType](currentAmount - amount);
            OnResourcesCountChanged?.Invoke(resType, currentAmount - amount);
            return true;
        }
    }

    public static void AddResource(ResourcesType resType, int amount)
    {
        int currentAmount = _getResources[resType]();

        _setResources[resType](currentAmount + amount);

        OnResourcesCountChanged?.Invoke(resType, currentAmount + amount);
    }

    public static int GetResource(ResourcesType resType) => _getResources[resType]();
}

public enum ResourcesType
{
    Gold = 0
}