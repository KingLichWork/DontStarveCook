using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class GameSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _spawnPoint;

    [SerializeField] private FoodData _foodData;

    private CancellationTokenSource _cts;

    private float _spawnTime;
    private float _toNextSpawnTime;

    public void SpawnStartFood(int count)
    {
        for (int i = 0; i < count; i++)
            SpawnFood();
    }

    public void StartSpawn(float timeSpawn)
    {
        _spawnTime = timeSpawn;
        _cts = new CancellationTokenSource();
        SpawnFood(_cts.Token).Forget();
    }

    private async UniTaskVoid SpawnFood(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);

            float delta = Time.deltaTime;

            _toNextSpawnTime += delta;

            if(_toNextSpawnTime >= _spawnTime)
            {
                _toNextSpawnTime = 0;
                SpawnFood();
            }
        }
    }

    public void StopSpawn()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }

    public void SpawnFood()
    {
        Food food = _foodData.GetRandomFood();

        FoodViewGame prefab = Instantiate(_prefab, _spawnPoint).GetComponent<FoodViewGame>();
        prefab.SetFood(food);
    }
}
