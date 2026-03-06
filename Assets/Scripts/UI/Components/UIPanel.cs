using UnityEngine;

public abstract class UIPanel : MonoBehaviour
{
    [SerializeField] private GameObject _holder;

    public void ChangeShowed()
    {
        if(_holder.activeInHierarchy)
            Hide();
        else
            Show();
    }

    public void Show()
    {
        _holder.SetActive(true);
        OnShow();
    }

    public void Hide()
    {
        _holder.SetActive(false);
        OnHide();
    }

    protected virtual void OnShow() { }

    protected virtual void OnHide() { }
}
