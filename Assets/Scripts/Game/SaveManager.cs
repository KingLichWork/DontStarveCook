using Kimicu.YandexGames;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SaveManager
{
    public static PlayerData PlayerData { get; private set; }

    private static bool _isNeedToSave = false;

    public static IEnumerator LoadAll()
    {
        ResetSaves(() =>
        {
            PlayerData = Load<PlayerData>(typeof(PlayerData).ToString()) ?? new PlayerData();

            _isNeedToSave = true;
        });

        yield return null;
    }
    public static void DeleteAllSave() => Cloud.ClearCloudData();

    public static T Load<T>(string key)
    {
        var save = Cloud.GetValue<T>(key);
        Debug.Log($"Load key {key}");
        return save;
    }

    public static void Save<T>(T save) where T : class
    {
        if (!_isNeedToSave) return;

        var key = typeof(T).ToString();
        Debug.Log($"Save key {key}");
        Cloud.SetValue(key, save);

        Cloud.SetValue("HasSaves", 1);
        Cloud.SaveInCloud();
    }

    private static void ResetSaves(Action onCallback = null)
    {
        var resetSaves = Cloud.GetValue("ResetSaves", 1) == 1;
        Debug.Log($"Reset saves is {resetSaves}");
        if (!resetSaves)
        {
            onCallback?.Invoke();
            return;
        }

        Cloud.ClearCloudData(onSuccessCallback: () =>
        {
            Debug.Log("Reset Saves complete");
            Cloud.SetValue("ResetSaves", 0);
            onCallback?.Invoke();
        });
    }
}

[Serializable]
public class PlayerData
{
    private int _score = 0;

    private int _health = 100;
    private int _hunger = 100;

    private int _maxHealth = 100;
    private int _maxHunger = 100;

    private int _day = 1;

    private bool _tutorial = false;

    public int Score { get { return _score; } set { _score = value; SaveManager.Save(this); } }
    public int Health { get { return _health; } set { _health = value; SaveManager.Save(this); } }
    public int Hunger { get { return _hunger; } set { _hunger = value; SaveManager.Save(this); } }
    public int MaxHealth { get { return _maxHealth; } set { _maxHealth = value; SaveManager.Save(this); } }
    public int MaxHunger { get { return _maxHunger; } set { _maxHunger = value; SaveManager.Save(this); } }

    public int Day { get { return _day; } set { _day = value; SaveManager.Save(this); } }

    public bool Tutorial { get { return _tutorial; } set { _tutorial = value; SaveManager.Save(this); } }
}