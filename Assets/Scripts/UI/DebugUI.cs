using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;

    [SerializeField] private Button _clearSavesButton;

    private void OnEnable()
    {
        _clearSavesButton.onClick.AddListener(ClearSaves);
    }

    private void OnDisable()
    {
        _clearSavesButton.onClick.RemoveListener(ClearSaves);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.D) && Input.GetKey(KeyCode.CapsLock) && Input.GetKey(KeyCode.LeftShift))
        {
            _gameObject.SetActive(!_gameObject.activeInHierarchy);
        }
    }

    private void ClearSaves()
    {
        SaveManager.DeleteAllSave();
    }
}
