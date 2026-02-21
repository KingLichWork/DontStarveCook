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

    public FoodViewGame CreateGameView(FoodView foodView, Vector3 worldPos)
    {
        var view = _objectResolver.Instantiate(_gamePrefab, worldPos, Quaternion.identity);
        view.SetFood(foodView.Food);

        if (foodView is FoodViewUI foodViewUI && foodViewUI.IsRecipe)
            view.transform.localScale = Vector3.one * 0.25f;

        return view;
    }

    public FoodViewUI CreateUIView(FoodBase food, Transform parent, bool isRecipe = false)
    {
        var view = _objectResolver.Instantiate(_uiPrefab, parent);
        view.SetFood(food, isRecipe);
        return view;
    }
}
