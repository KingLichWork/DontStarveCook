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
    }

    public void Hide()
    {
        _holder.SetActive(false);
    }
}
