using UnityEngine;
using VContainer;
using VContainer.Unity;

public class FoodViewFactory
{
    private FoodViewGame _gamePrefab;
    private FoodViewUI _uiPrefab;
    private Camera _camera;
    private GameUI _gameUI;

    private IObjectResolver _objectResolver;
    
    public FoodViewFactory(FoodViewGame gamePrefab, FoodViewUI uiPrefab, GameUI gameUI, IObjectResolver objectResolver)
    {
        _gamePrefab = gamePrefab;
        _uiPrefab = uiPrefab;
        _objectResolver = objectResolver;
        _gameUI = gameUI;
        _camera = Camera.main;

        InputController.DragFoodViewAction += CreateUIViewDrag;
    }

    public FoodViewGame CreateGameView(FoodView foodView, Vector3 worldPos)
    {
        var view = _objectResolver.Instantiate(_gamePrefab, worldPos, Quaternion.identity);
        view.SetFood(foodView.Food);

        if (foodView is FoodViewUI foodViewUI && foodViewUI.IsRecipe)
            view.transform.localScale = Vector3.one * 0.3f;

        return view;
    }

    public FoodViewUI CreateUIView(FoodBase food, Transform parent, bool isRecipe = false)
    {
        var view = _objectResolver.Instantiate(_uiPrefab, parent);
        view.SetFood(food, isRecipe);
        return view;
    }

    private FoodViewUI CreateUIViewDrag(FoodBase food, Vector3 position, bool isRecipe = false)
    {
        var view = _objectResolver.Instantiate(_uiPrefab, _camera.WorldToScreenPoint(position), Quaternion.identity, _gameUI.FoodViewParent);
        view.SetFood(food, isRecipe);
        return view;
    }
}
