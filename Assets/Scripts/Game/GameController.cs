using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameSpawner _spawner;

    private GameTimer _gameTimer = new GameTimer(100);

    private void Start()
    {
        Game();
    }

    private void OnEnable()
    {
        FoodView.EatFoodAction += EatFood;
    }

    private void OnDisable()
    {
        FoodView.EatFoodAction -= EatFood;
    }

    public async UniTask Game()
    {
        _gameTimer.StartTimer();

        while (true)
        {
            await UniTask.Delay(1000);
            _spawner.SpawnFood();
        }
    }

    private void EatFood(FoodView foodView)
    {
        _gameTimer.ChangeTimer(foodView.Food.FoodValue);
        Destroy(foodView);
    }
}
