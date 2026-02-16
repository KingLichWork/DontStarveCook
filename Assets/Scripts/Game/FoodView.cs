using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FoodView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private PolygonCollider2D _collider;

    private Food _food;

    public Food Food => _food;

    public static event Action<FoodView> UseFoodAction;
    public static event Action<FoodView> EatFoodAction;

    public void Init(Food food)
    {
        _food = food;
        _sprite.sprite = _food.Sprite;

        RefreshCollider();
    }

    private void RefreshCollider()
    {
        _collider.pathCount = _sprite.sprite.GetPhysicsShapeCount();

        for (int i = 0; i < _collider.pathCount; i++)
        {
            var points = new List<Vector2>();
            _sprite.sprite.GetPhysicsShape(i, points);
            _collider.SetPath(i, points.ToArray());
        }
    }

    public void EatFood()
    {
        EatFoodAction.Invoke(this);
    }
}
