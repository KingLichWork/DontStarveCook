using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private float _longPressTime = 0.5f;

    private FoodView _hoveredFood;

    private bool _isPointerDown = false;
    private float _pointerDownTimer = 0f;
    private FoodView _pressedFood;

    private void Update()
    {
        HandleMouse();
        HandleTouch();

        if (_isPointerDown)
        {
            _pointerDownTimer += Time.deltaTime;
            if (_pointerDownTimer >= _longPressTime)
            {
                _isPointerDown = false;
                _pointerDownTimer = 0f;

                _pressedFood?.ShowDescription();
            }
        }
    }

    private void HandleMouse()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hitHover = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
        if (hitHover.collider != null)
        {
            var food = hitHover.collider.GetComponent<FoodView>();
            if (_hoveredFood != food)
            {
                _hoveredFood?.HideDescription();
                _hoveredFood = food;
                _hoveredFood.ShowDescription();
            }
        }
        else
        {
            _hoveredFood?.HideDescription();
            _hoveredFood = null;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hitClick = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
            if (hitClick.collider != null)
            {
                var food = hitClick.collider.GetComponent<FoodView>();
                food?.EatFood();
            }
        }
#endif
    }

    private void HandleTouch()
    {
#if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(touch.position);

        if (touch.phase == TouchPhase.Began)
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            if (hit.collider != null)
            {
                _pressedFood = hit.collider.GetComponent<FoodView>();
                _isPointerDown = true;
                _pointerDownTimer = 0f;
            }
        }
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            if (_isPointerDown && _pointerDownTimer < longPressTime)
            {
                _pressedFood?.Eat();
            }

            _isPointerDown = false;
            _pointerDownTimer = 0f;

            _pressedFood?.HideDescription();
            _pressedFood = null;
        }
#endif
    }
}
