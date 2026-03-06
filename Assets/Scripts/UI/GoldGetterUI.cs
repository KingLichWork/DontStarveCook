using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class GoldGetterUI : MonoBehaviour
{
    [SerializeField] private RectTransform _gettedGoldObject;
    [SerializeField] private TextMeshProUGUI _goldGetted;

    public void GetGold(FoodView food)
    {
        int value = Mathf.Abs(food.Food.FoodValue + food.Food.HealthValue);

        ResourcesWallet.AddResource(ResourcesType.Gold, value);
        GetGoldEffect(value);
    }

    private void GetGoldEffect(int value)
    {
        _gettedGoldObject.DOKill();
        _gettedGoldObject.gameObject.SetActive(true);

        float startY = _gettedGoldObject.position.y;
        _goldGetted.text = $"+{value}";

        _gettedGoldObject.DOMoveY(startY + 10f, 0.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _gettedGoldObject.position = new Vector3(
                    _gettedGoldObject.position.x,
                    startY,
                    _gettedGoldObject.position.z);

                _gettedGoldObject.gameObject.SetActive(false);
            });
    }
}
