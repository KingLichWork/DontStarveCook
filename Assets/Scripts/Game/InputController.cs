using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

public class InputController : MonoBehaviour
{
    private GraphicRaycaster _graphicRaycaster;
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
    private bool _isMobile;

    public static event Action<FoodView> ShowDescriptionAction;
    public static event Action<FoodView> HideDescriptionAction;

    public static event Func <FoodBase, Vector3, bool, FoodViewUI> DragFoodViewAction;
    public static event Action<FoodView, Vector2> DropAction;

    [Inject]
    public void Construct(GraphicRaycaster graphicRaycaster)
    {
        _graphicRaycaster = graphicRaycaster;
        _camera = Camera.main;
    }

    public void Init()
    {
#if !UNITY_EDITOR
        _isMobile = Kimicu.YandexGames.Device.IsMobile;
#else
        _isMobile = false;
#endif
    }

    private void Update()
    {
        if(_isMobile)
            HandleTouch();
        else 
            HandleMouse();
    }

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
            _draggedFood.EndDrag();

            DropAction?.Invoke(_draggedFood, worldPos);
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

        if (_draggedFood is FoodViewGame view)
            CreateUIView(view);

        _draggedFood.StartDrag(worldPos);
        HideDescriptionAction?.Invoke(_draggedFood);
    }

    private void CreateUIView(FoodViewGame view)
    {
        FoodViewUI uiView = DragFoodViewAction?.Invoke(view.Food, view.transform.position, view.IsRecipe);

        uiView.Init(view);
        _draggedFood.Hide();

        _pressedFood = _draggedFood = uiView;
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
