using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FoodView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private PolygonCollider2D _collider;
    [SerializeField] private Rigidbody2D _rb2D;

    private Food _food;

    private Vector3 _startPosition;

    public Food Food => _food;

    public static event Action<FoodView> UseFoodAction;
    public static event Action<FoodView> EatFoodAction;

    public void SetFood(Food food)
    {
        _food = food;
        _sprite.sprite = _food.Sprite;

        RefreshCollider();
    }

    public void StartDrag()
    {
        _startPosition = transform.position;
        _rb2D.bodyType = RigidbodyType2D.Kinematic;
    }

    public void ReturnToStartPosition()
    {
        transform.position = _startPosition;
        _rb2D.bodyType = RigidbodyType2D.Dynamic;
    }

    public void EnableRB()
    {
        _rb2D.bodyType = RigidbodyType2D.Dynamic;
    }

    public void EatFood()
    {
        EatFoodAction.Invoke(this);
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
}
