using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    [SerializeField] private GraphicRaycaster _graphicRaycaster;

    private Camera _camera;

    private const float LongPressTime = 0.5f;
    private const float DragThreshold = 0.1f;

    private FoodView _selectedFood;
    private FoodView _pressedFood;
    private FoodView _draggedFood;

    private Vector2 _pressStartWorldPos;
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
        Vector2 screenPos = Input.mousePosition;

        HandleHover(screenPos);

        if (Input.GetMouseButtonDown(0))
            BeginPress(screenPos);

        if (Input.GetMouseButton(0))
            UpdatePress(screenPos);

        if (Input.GetMouseButtonUp(0))
            EndPress(screenPos);
    }

    #endregion

    #region Touch

    private void HandleTouch()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);
        Vector2 screenPos = touch.position;

        if (touch.phase == TouchPhase.Began)
            BeginPress(screenPos);

        if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            UpdatePress(screenPos);

        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            EndPress(screenPos);
    }

    #endregion

    private void HandleHover(Vector2 screenPos)
    {
        if (_isDragging) return;

        FoodView food = RaycastFood(screenPos);

        if (_selectedFood == food)
            return;

        if (_selectedFood != null)
            HideDescriptionAction?.Invoke(_selectedFood);

        _selectedFood = food;

        if (_selectedFood != null)
            ShowDescriptionAction?.Invoke(_selectedFood);
    }

    private void BeginPress(Vector2 screenPos)
    {
        _pressedFood = RaycastFood(screenPos);
        if (_pressedFood == null)
            return;

        _pressStartWorldPos = ScreenToWorld(screenPos);
        _pressTimer = 0f;
        _isPressing = true;
        _isDragging = false;
    }

    private void UpdatePress(Vector2 screenPos)
    {
        if (!_isPressing)
            return;

        Vector2 worldPos = ScreenToWorld(screenPos);
        float distance = Vector2.Distance(_pressStartWorldPos, worldPos);

        if (!_isDragging && distance > DragThreshold)
        {
            StartDrag(worldPos);
        }

        if (_isDragging)
        {
            _draggedFood.Drag(worldPos);
            return;
        }

        _pressTimer += Time.deltaTime;

        if (_pressTimer >= LongPressTime)
        {
            ShowDescriptionAction?.Invoke(_pressedFood);
            _isPressing = false;
        }
    }

    private void EndPress(Vector2 screenPos)
    {
        Vector2 worldPos = ScreenToWorld(screenPos);

        if (_isDragging)
        {
            _draggedFood.EndDrag(worldPos);
            TryDrop(worldPos);
        }
        else if (_isPressing)
        {
            _pressedFood?.Eat();
        }

        ResetState();
    }

    private void StartDrag(Vector2 worldPos)
    {
        _isDragging = true;
        _draggedFood = _pressedFood;

        _draggedFood.StartDrag(worldPos);
        HideDescriptionAction?.Invoke(_draggedFood);
    }

    private void TryDrop(Vector2 worldPos)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.zero);

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
                    _draggedFood.EndDrag(worldPos);
                    break;
            }
        }

        _draggedFood.EndDrag(worldPos);
    }

    private void TryDropOnStation(Collider2D collider)
    {
        CookingStation station = collider.GetComponent<CookingStation>();

        if (station == null || !station.TryCook(_draggedFood))
            _draggedFood.ReturnToStartPosition();
    }

    private FoodView RaycastFood(Vector2 screenPos)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = screenPos
        };

        var results = new List<RaycastResult>();
        _graphicRaycaster.Raycast(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent<FoodView>(out var uiFood))
                return uiFood;
        }

        Vector2 worldPos = ScreenToWorld(screenPos);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider &&
            hit.collider.TryGetComponent<FoodView>(out var worldFood))
        {
            return worldFood;
        }

        return null;
    }

    private Vector2 ScreenToWorld(Vector2 screenPos)
    {
        return _camera.ScreenToWorldPoint(screenPos);
    }

    private void ResetState()
    {
        _isPressing = false;
        _isDragging = false;
        _draggedFood = null;
        _pressedFood = null;
    }
}
