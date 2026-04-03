using System;
using System.Collections;
using Agava.YandexGames;
using GameAnalyticsSDK;
using Kimicu.YandexGames.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using Billing = Kimicu.YandexGames.Billing;

namespace Utils
{
    public static class AdManager
    {
        private static float _lastTimeScale;
        private static bool _inAd;
        public static bool IsAdAvailable => Kimicu.YandexGames.Advertisement.AdvertisementIsAvailable;

        public static void ShowInterstitial(Action onOpen = null, Action onClose = null, Action<string> onError = null, bool ignoreAdClicker = false)
        {
            if (Kimicu.YandexGames.Advertisement.AdvertisementIsAvailable && !SaveManager.PlayerData.NoAds && !_inAd)
            {
                _inAd = true;

                var result = "0";
#if UNITY_WEBGL && !UNITY_EDITOR
                result = GameAnalytics.GetRemoteConfigsValueAsString("Clicker", "0");
#endif

                if (ignoreAdClicker)
                {
                    ShowInter(onOpen, onClose, onError);
                }
                else
                {
                    ShowInterClicker(result.Equals("1"), onOpen, onClose, onError);
                }
            }
            else
            {
                Debug.Log($"Cant show inter. AdvertisementIsAvailable: {Kimicu.YandexGames.Advertisement.AdvertisementIsAvailable}. "
                          + $"No ads: {SaveManager.PlayerData.NoAds}. In Ad: {_inAd}");

                onClose?.Invoke();
            }
        }

        public static void ShowRewarded(Action onOpen = null, Action onRewarded = null, Action onClose = null, Action<string> onError = null)
        {
            if (_inAd)
                return;

            _inAd = true;

            var result = "0";
#if UNITY_WEBGL && !UNITY_EDITOR
            result = GameAnalytics.GetRemoteConfigsValueAsString("Clicker", "0");
#endif

            ShowRewardClicker(result.Equals("1"), onOpen, onRewarded, onClose, onError);
        }

        private static void ShowInter(Action onOpen = null, Action onClose = null, Action<string> onError = null)
        {
            onOpen = SubscribeOnOpen(onOpen);
            onClose = SubscribeOnClose(onClose);

            Debug.Log("Inter");

            /*#if UNITY_EDITOR
                        onOpen?.Invoke();
                        onClose?.Invoke();
                        return;
            #endif*/
            Kimicu.YandexGames.Advertisement.ShowInterstitialAd(onOpen, () => onClose?.Invoke(), onError);
        }

        private static void ShowInterClicker(bool isMinigame, Action onOpen = null, Action onClose = null, Action<string> onError = null)
        {
            OnAdClickerOpened();

            Coroutines.StartRoutine(PreAdScreen.Instance.AdTimer(() => ShowInter(() =>
            {
                onOpen?.Invoke();
                PreAdScreen.Instance.StopField();
            }, onClose, onError), isMinigame));
        }

        private static void ShowRewardAd(Action onOpen = null, Action onRewarded = null, Action onClose = null, Action<string> onError = null)
        {
            onOpen = SubscribeOnOpen(onOpen);
            onClose = SubscribeOnClose(onClose);

            /*#if UNITY_EDITOR
                        onOpen?.Invoke();
                        onRewarded?.Invoke();
                        onClose?.Invoke();
                        return;
            #endif*/
            Kimicu.YandexGames.Advertisement.ShowVideoAd(onOpen, onRewarded, () => onClose?.Invoke(), onError);
        }

        private static void ShowRewardClicker(bool isMinigame, Action onOpen = null, Action onRewarded = null, Action onClose = null, Action<string> onError = null)
        {
            OnAdClickerOpened();
            //onClose = SubscribeOnClose(onClose);

            Coroutines.StartRoutine(PreAdScreen.Instance.AdTimer(() => ShowRewardAd(() =>
            {
                onOpen?.Invoke();
                PreAdScreen.Instance.StopField();
            }, onRewarded, onClose, onError), isMinigame));
        }

        private static Action SubscribeOnOpen(Action onOpen)
        {
            if (onOpen == null)
            {
                onOpen += AdStart;
            }
            else
            {
                onOpen = AdStart + onOpen;
            }

            return onOpen;
        }

        private static Action SubscribeOnClose(Action onClose)
        {
            if (onClose == null)
            {
                onClose += AdEnd;
            }
            else
            {
                onClose = AdEnd + onClose;
            }

            onClose += OnAdClickerClosed;

            return onClose;
        }

        private static void OnAdClickerOpened()
        {
            Time.timeScale = 0;
        }

        private static void OnAdClickerClosed()
        {
            Time.timeScale = 1;
        }

        private static void AdStart()
        {
            Debug.Log("Ad started");
            if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);

            AudioListener.pause = true;

            _lastTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        private static void AdEnd()
        {
            _inAd = false;
            Debug.Log("Ad ended");

            AudioListener.pause = false;

            if (_lastTimeScale != 0)
                Time.timeScale = _lastTimeScale;
            else
                Time.timeScale = 1;
        }

        public static void BuyPurchase(string purchaseID, Action<PurchaseProductResponse> onPurchase = null, Action<string> onError = null, Action onConsume = null)
        {
            Billing.PurchaseProduct(purchaseID, (p) =>
            {
                onPurchase?.Invoke(p);
                Billing.ConsumeProduct(purchaseID, () =>
                {
                    onConsume?.Invoke();
                });
            }, (e) =>
            {
                Debug.LogError("BuyPurchase Error: " + e);
                onError?.Invoke(e);
            });
        }

        private static IEnumerator WaitWhileCanShowAd()
        {
            if (Kimicu.YandexGames.Advertisement.AdvertisementIsAvailable)
            {
                yield return new WaitWhile(() => Kimicu.YandexGames.Advertisement.AdvertisementIsAvailable);
            }

            yield return new WaitUntil(() => Kimicu.YandexGames.Advertisement.AdvertisementIsAvailable);
        }
    }

    public static class PurchaseID
    {
        public static string AdBlockID = "NoAds";
    }
}