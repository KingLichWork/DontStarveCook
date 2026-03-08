using FindTheDifference.PreAdClicker.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PreAdClicker : MonoBehaviour
{
    //private const string _VFX_PATH = "Vfx/PowerupGlow";

    [SerializeField] private Transform _scoreTarget;
    [SerializeField] private TextMeshProUGUI _scoreCounter;

    [SerializeField] private float _spawnDelay;
    [SerializeField] private RectTransform _spawnZone;
    [SerializeField] private ClickObject _clickObjectPrefab;

    [SerializeField] private Transform _tutorialHand;
    [SerializeField] private GameObject _tutorialText;

    private readonly List<GameObject> _instantiatedObjects = new List<GameObject>();

    private int _score;

    private int _spriteIndex = -1;

    private int _scoreForTap;

    private bool _spawning;

    public void StartField()
    {
        Debug.Log("Clicked started");
        _spawning = true;
        _scoreCounter.text = $"{_score}";

        StartCoroutine(SpawnObjects());

        _tutorialHand.gameObject.SetActive(true);
        _tutorialText.gameObject.SetActive(true);
    }

    public void StopField()
    {
        Debug.Log("Clicked finished");
        _spawning = false;
        ResetField();

        //UIManager.Instance.SetCountStars();
    }

    private void ResetField()
    {
        _score = 0;
        _scoreCounter.text = $"{_score}";

        foreach (var go in _instantiatedObjects.Where(go => go != null))
        {
            Destroy(go);
        }
    }

    private void OnClicked(ClickObject clickObject)
    {
        clickObject.Clicked -= OnClicked;

        _score++;
        _scoreCounter.text = $"{_score}";

        ResourcesWallet.AddResource(ResourcesType.Gold, 1);

        if (_tutorialText.activeSelf || _tutorialHand.gameObject.activeSelf)
        {
            _tutorialText.SetActive(false);
            _tutorialHand.gameObject.SetActive(false);
        }

    }

    private IEnumerator SpawnObjects()
    {
        var index = Random.Range(0, 4);
        _spriteIndex = _spriteIndex == index ? Random.Range(0, 4) : index;
        while (_spawning)
        {
            var clickObject = Instantiate(_clickObjectPrefab, _spawnZone);
            clickObject.Init(_spriteIndex);
            clickObject.Clicked += OnClicked;

            var clickObjectRt = clickObject.transform as RectTransform;

            Vector2 clickObjectPosition;

            var leftBounds = _spawnZone.anchoredPosition.x - _spawnZone.rect.width / 2 + clickObjectRt.rect.width / 2;
            var rightBounds = -leftBounds;

            var topBounds = _spawnZone.anchoredPosition.y + _spawnZone.rect.height / 2 - clickObjectRt.rect.height / 2;
            var bottomBounds = clickObjectRt.rect.height / 2;

            clickObjectPosition.x = Random.Range(leftBounds, rightBounds);
            clickObjectPosition.y = Random.Range(bottomBounds, topBounds);

            clickObjectRt.anchoredPosition = clickObjectPosition;

            _instantiatedObjects.Add(clickObject.gameObject);

            yield return new WaitForSecondsRealtime(_spawnDelay);
        }
    }
}

