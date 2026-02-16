using System;
using UnityEngine;

[Serializable]
public class Food
{
    [SerializeField] private int _foodValue;
    [SerializeField] private int _decayTime;
    [SerializeField] private string _foodName;
    [SerializeField] private FoodType _type;
    [SerializeField] private Sprite _sprite;

    public int FoodValue => _foodValue;
    public int DecayTime => _decayTime;
    public string Name => _foodName;
    public FoodType Type => _type;
    public Sprite Sprite => _sprite;
}

public enum FoodType
{
    meat,
    fish,
    vegetable,
    fruit,
    egg
}

