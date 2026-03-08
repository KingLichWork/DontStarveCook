using System;
using System.Collections;
using Agava.YandexGames;
using UnityEngine;

namespace Utils
{
    public static class Purchase
    {
        private static PurchasedProduct[] _purchasedProducts;
        private static CatalogProduct[] _catalogProducts;

        public static IEnumerator Initialize()
        {
#if UNITY_EDITOR
            Debug.Log("Purhcases in editor inited");
            yield break;
#endif
            Kimicu.YandexGames.Billing.GetPurchasedProducts((r) => _purchasedProducts = r.purchasedProducts);
            _catalogProducts = Kimicu.YandexGames.Billing.CatalogProducts;
            Debug.Log("Purhcases in build inited");
            yield return null;
        }

        public static void BuyPurchase(string purchaseID, Action<PurchaseProductResponse> onPurchase = null, Action<string> onError = null, Action onConsume = null)
        {
#if UNITY_EDITOR
            //PurchaseProductResponse p = new PurchaseProductResponse();
            onConsume?.Invoke();
            Debug.Log("Purhcased in editor");
            return;
#endif
            Kimicu.YandexGames.Billing.PurchaseProduct(purchaseID, (p) =>
            {
                onPurchase?.Invoke(p);
                Debug.Log("Success purchase with ID: " + purchaseID);
                Kimicu.YandexGames.Billing.ConsumeProduct(p.purchaseData.purchaseToken, () =>
                {
                    Debug.Log("Success consume with ID: " + purchaseID);
                    onConsume?.Invoke();
                }, (s) => Debug.Log(s));
            }, (e) =>
            {
                Debug.LogError("Buy Purchase Error: " + e);
                onError?.Invoke(e);
            });
        }

        public static void CheckConsume(string purchaseID, Action onSuccess = null, Action onError = null)
        {
#if UNITY_EDITOR
            Debug.Log($"Purhchase checked with ID: {purchaseID}");
            return;
#endif
            if (_purchasedProducts == null || _purchasedProducts.Length == 0)
            {
                Debug.Log("_purchasedProducts is null or 0");
                return;
            }
            foreach (var purchase in _purchasedProducts)
            {
                if (purchase.productID == purchaseID)
                {
                    Kimicu.YandexGames.Billing.ConsumeProduct(purchase.purchaseToken,
                        onSuccessCallback: () => onSuccess.Invoke(),
                        onErrorCallback: (s) =>
                        {
                            Debug.Log("Error while consume product with ID: " + purchase.productID);
                            Debug.Log(s);
                            onError.Invoke();
                        });
                }
            }
        }
    }
}
