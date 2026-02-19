using System;
using UnityEngine;

public abstract class FoodView : MonoBehaviour
{
    protected Food _food;

    public Food Food => _food;

    public static event Action<FoodView> EatFoodAction;

    public virtual void SetFood(Food food)
    {
        _food = food;
    }

    public virtual void Eat()
    {
        EatFoodAction.Invoke(this);
    }

    public virtual void EndDrag(){}

    public abstract void Drag(Vector2 pointerPos);

    public abstract void StartDrag(Vector2 pointerWorldPos);

    public abstract void ReturnToStartPosition();

    public abstract Vector3 GetDescriptionPosition();
}