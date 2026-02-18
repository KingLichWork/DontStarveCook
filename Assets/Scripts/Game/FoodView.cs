using System;
using UnityEngine;

public class FoodView : MonoBehaviour
{
    protected Food _food;

    public Food Food => _food;

    public static event Action<FoodView> EatFoodAction;

    public virtual void SetFood(Food food)
    {
        _food = food;
    }

    public virtual void StartDrag(Vector2 pointerWorldPos)
    {

    }

    public virtual void Drag(Vector2 pointerWorldPos)
    {
        transform.position = pointerWorldPos;
    }

    public virtual void Eat()
    {
        EatFoodAction.Invoke(this);
    }

    public virtual void EndDrag(Vector2 pointerWorldPos)
    {

    }

    public virtual void ReturnToStartPosition()
    {}
}