using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FoodViewGame : FoodView
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private PolygonCollider2D _collider;
    [SerializeField] private Rigidbody2D _rb2D;

    private Vector3 _startPosition;

    public override void SetFood(Food food)
    {
        base.SetFood(food);
        _sprite.sprite = _food.Sprite;

        RefreshCollider();
    }

    public override void StartDrag(Vector2 pointerWorldPos)
    {
        _startPosition = transform.position;
        _rb2D.bodyType = RigidbodyType2D.Kinematic;
    }

    public override void EndDrag(Vector2 pointerWorldPos)
    {
        EnableRB();
    }

    public override void ReturnToStartPosition()
    {
        transform.position = _startPosition;
        _rb2D.bodyType = RigidbodyType2D.Dynamic;
    }

    public void EnableRB()
    {
        _rb2D.bodyType = RigidbodyType2D.Dynamic;
    }

    public override void Eat()
    {
        base.Eat();
        Destroy(gameObject);
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
