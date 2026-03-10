using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

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
        _timerImage.DOKill();
        _timerImage.DOFillAmount(value, 1f);
    }

    protected void ChangeCookStage()
    {
        _timerImage.color = new Color(1, 0, 0, 0.3f);
        ClearCook();
    }

    protected void DoClearCook()
    {
        _timerImage.DOKill();
        _timerImage.DOFillAmount(0, 0.5f);
    }

    protected void ClearCook()
    {
        _timerImage.DOKill();
        _timerImage.fillAmount = 0;
    }
}
