using UnityEngine;

public class GameSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _spawnPoint;

    [SerializeField] private FoodData _foodData;

    public void SpawnFood()
    {
        Food food = _foodData.Food[Random.Range(0, _foodData.Food.Count)];

        FoodView prefab = Instantiate(_prefab, _spawnPoint).GetComponent<FoodView>();
        prefab.Init(food);
    }
}
