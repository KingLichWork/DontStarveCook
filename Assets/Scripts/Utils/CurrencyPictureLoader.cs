using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    [RequireComponent(typeof(Image))]
    public class CurrencyPictureLoader : MonoBehaviour
    {
        private Image _image;

        private void Awake() => _image = GetComponent<Image>();

        private void OnEnable()
        {
            _image.sprite = Currency.CurrencyPictures.FirstOrDefault().currency.picture;
            if (_image.sprite != null) return;

            Debug.LogWarning($"picture not found. Id out of frist purchase: {Currency.CurrencyPictures.FirstOrDefault().id}");
            _image.sprite = Resources.Load<Sprite>("Yandex");
        }
    }
}
