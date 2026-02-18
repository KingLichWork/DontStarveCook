using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private float _longPressTime = 0.5f;
    private float _dragThreshold = 0.1f;

    private Camera _camera;

    private FoodView _selectedFood;
    private FoodView _pressedFood;
    private FoodView _draggedFood;

    private Vector2 _pressStartWorldPos;
    private Vector3 _dragOffset;

    private float _pressTimer;
    private bool _isPressing;
    private bool _isDragging;

    public static event Action<FoodView> ShowDescriptionAction;
    public static event Action<FoodView> HideDescriptionAction;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouse();
#endif

#if UNITY_IOS || UNITY_ANDROID
        HandleTouch();
#endif
    }

    #region Mouse

    private void HandleMouse()
    {
        Vector2 worldPos = GetWorldPosition(Input.mousePosition);

        HandleHover(worldPos);

        if (Input.GetMouseButtonDown(0))
            BeginPress(worldPos);

        if (Input.GetMouseButton(0))
            UpdatePress(worldPos);

        if (Input.GetMouseButtonUp(0))
            EndPress(worldPos);
    }

    #endregion

    #region Touch

    private void HandleTouch()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);
        Vector2 worldPos = GetWorldPosition(touch.position);

        if (touch.phase == TouchPhase.Began)
            BeginPress(worldPos);

        if (touch.phase == TouchPhase.Moved)
            UpdatePress(worldPos);

        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            EndPress(worldPos);
    }

    #endregion

    private Vector2 GetWorldPosition(Vector2 screenPos)
    {
        return _camera.ScreenToWorldPoint(screenPos);
    }

    private void HandleHover(Vector2 worldPos)
    {
        if (_isDragging) return;

        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
        FoodView food = hit.collider ? hit.collider.GetComponent<FoodView>() : null;

        if (_selectedFood == food) return;

        HideDescriptionAction?.Invoke(_selectedFood);
        _selectedFood = food;
        ShowDescriptionAction?.Invoke(_selectedFood);
    }

    private void BeginPress(Vector2 worldPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
        if (!hit.collider) return;

        _pressedFood = hit.collider.GetComponent<FoodView>();
        if (_pressedFood == null) return;

        _pressStartWorldPos = worldPos;
        _pressTimer = 0f;
        _isPressing = true;
        _isDragging = false;

        _dragOffset = _pressedFood.transform.position - (Vector3)worldPos;
    }

    private void UpdatePress(Vector2 worldPos)
    {
        if (!_isPressing) return;

        float distance = Vector2.Distance(_pressStartWorldPos, worldPos);

        if (!_isDragging && distance > _dragThreshold)
        {
            StartDrag();
        }

        if (_isDragging)
        {
            _draggedFood.transform.position = worldPos + (Vector2)_dragOffset;
            return;
        }

        _pressTimer += Time.deltaTime;
        if (_pressTimer >= _longPressTime)
        {
            ShowDescriptionAction?.Invoke(_pressedFood);
            _isPressing = false;
        }
    }

    private void EndPress(Vector2 worldPos)
    {
        if (_isDragging)
        {
            TryDrop(worldPos);
        }
        else if (_isPressing)
        {
            _pressedFood?.EatFood();
        }

        _isPressing = false;
        _isDragging = false;
        _draggedFood = null;
        _pressedFood = null;
    }

    private void StartDrag()
    {
        _isDragging = true;
        _draggedFood = _pressedFood;
        _draggedFood.StartDrag();

        HideDescriptionAction?.Invoke(_draggedFood);
    }

    private void TryDrop(Vector2 worldPos)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector3.forward);

        foreach (var hit in hits)
        {
            switch (hit.collider.tag)
            {
                case "Block":
                    _draggedFood.ReturnToStartPosition();
                    break;
                case "SingleCook":
                    TryDropOnStation(hit.collider);
                    break;
                case "MultiCook":
                    break;
                default:
                    _draggedFood.EnableRB();
                    break;
            }
        }
    }

    private void TryDropOnStation(Collider2D collider)
    {
        CookingStation station = collider.GetComponent<CookingStation>();

        if (station == null || !station.TryCook(_draggedFood))
        {
            _draggedFood.ReturnToStartPosition();
        }
    }
}
