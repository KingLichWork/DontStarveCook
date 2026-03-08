using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;
using DG.Tweening;

namespace FindTheDifference.PreAdClicker.Scripts
{
    public class ClickObject : MonoBehaviour, IPointerDownHandler
    {
        [Header("Скорость в процентах высоты экрана в секунду")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private GameObject _image;
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private TextMeshProUGUI _text;

        private RectTransform _rt;

        public event Action<ClickObject> Clicked;

        public void Init(int index)
        {
            _rt = transform as RectTransform;

            var image = GetComponent<Image>();

        }

        private void Update()
        {
            _rt.anchoredPosition += Vector2.up * (Screen.height * (_moveSpeed / 100) * Time.unscaledDeltaTime);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Clicked?.Invoke(this);
            gameObject.GetComponent<Image>().raycastTarget = false;
            _image.SetActive(false);
            _text.gameObject.SetActive(true);
            _text.DOFade(0, 0.5f);

            _particles.Play();

            //AudioManager.Instance.PlayOneShot(SoundType.ButtonSound);
        }
    }
}
