using UnityEngine;
using UnityEngine.UI;

public class CookStationUI : MonoBehaviour
{
    [SerializeField] protected Image _timerImage;
    [SerializeField] private GameObject _holder;

    [SerializeField] private Transform _viewParent;

    public Transform ViewParent => _viewParent;

    protected void StartCook()
    {
        _timerImage.color = new Color(0, 1, 0, 0.3f);
        ClearCook();
    }

    protected void ChangeCookTimer(float value)
    {
        _timerImage.fillAmount = value;
    }

    protected void ChangeCookStage()
    {
        _timerImage.color = new Color(1, 0, 0, 0.3f);
        ClearCook();
    }

    protected void ClearCook()
    {
        _timerImage.fillAmount = 0;
    }
}
