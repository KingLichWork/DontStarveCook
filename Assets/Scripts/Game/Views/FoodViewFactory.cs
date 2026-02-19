using UnityEngine;
using VContainer;
using VContainer.Unity;

public class FoodViewFactory
{
    private FoodViewGame _gamePrefab;
    private FoodViewUI _uiPrefab;

    private IObjectResolver _objectResolver;
    
    public FoodViewFactory(FoodViewGame gamePrefab, FoodViewUI uiPrefab, IObjectResolver objectResolver)
    {
        _gamePrefab = gamePrefab;
        _uiPrefab = uiPrefab;
        _objectResolver = objectResolver;
    }

    public FoodViewGame CreateGameView(Food food, Vector3 worldPos)
    {
        var view = _objectResolver.Instantiate(_gamePrefab, worldPos, Quaternion.identity);
        view.SetFood(food);
        return view;
    }

    public FoodViewUI CreateUIView(Food food, Transform parent)
    {
        var view = _objectResolver.Instantiate(_uiPrefab, parent);
        view.SetFood(food);
        return view;
    }
}
