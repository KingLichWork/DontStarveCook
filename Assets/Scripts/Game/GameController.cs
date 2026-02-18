using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameSpawner _spawner;

    private GameTimer _gameTimer = new GameTimer(100);

    private void Start()
    {
        Game();

        for (int i = 0; i < 10; i++)
            _spawner.SpawnFood();
    }

    private void OnEnable()
    {
        FoodView.EatFoodAction += EatFood;
    }

    private void OnDisable()
    {
        FoodView.EatFoodAction -= EatFood;
    }

    public void Game()
    {
        _gameTimer.StartTimer();
        //_spawner.StartSpawn(1f);
    }

    private void EatFood(FoodView foodView)
    {
        _gameTimer.ChangeTimer(foodView.Food.FoodValue);
        Destroy(foodView.gameObject);
    }
}
