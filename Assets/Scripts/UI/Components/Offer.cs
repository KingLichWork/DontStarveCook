using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class Offer : MonoBehaviour
{
    [SerializeField] protected Button _button;

    [SerializeField] private string _buyKey;

    public string BuyKey => _buyKey;

    public void Init(Action onClick, bool isActive = true)
    {
        if (isActive)
        {
            gameObject.SetActive(false);
            return;
        }

        _button.onClick.AddListener(() =>
        {
            Purchase.BuyPurchase(_buyKey, onConsume: () => onClick.Invoke());
        });

        Purchase.CheckConsume(_buyKey, () => onClick.Invoke());
    }

    private void OnDestroy() => _button.onClick.RemoveAllListeners();
}
