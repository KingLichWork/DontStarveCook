using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using VContainer;

namespace FindTheDifference.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicAudioSource;
        [SerializeField] private AudioSource _soundsAudioSource;

        [SerializeField] private Sounds _sounds;

        private Coroutine _changeMusic;
        private GameUI _gameUI;

        public static bool IsVolumeActive = true;

        [Inject]
        public void Construct(GameUI gameUI)
        {
            _gameUI = gameUI;
            Init();
        }

        private void OnEnable()
        {
            _gameUI.ChangeSoundAction += SetVolume;
            _gameUI.ExtractAction += ClickButton;
        }

        private void OnDisable()
        {
            _gameUI.ChangeSoundAction -= SetVolume;
            _gameUI.ExtractAction -= ClickButton;
        }

        public void Init()
        {
            PlayMusic(AudioType.MainMusic, 0, 0.1f);
        }

        public void SetVolume(bool isOn)
        {
            IsVolumeActive = isOn;

            if (isOn)
                AudioListener.volume = 1;
            else
                AudioListener.volume = 0;
        }

        public void PlayMusic(AudioType music, int delay = 0, float volume = 1)
        {
            if (_changeMusic != null)
            {
                StopCoroutine(_changeMusic);
                _changeMusic = null;
            }
            _changeMusic = StartCoroutine(ChangeMusic(music, delay, volume));
        }

        public void ClickButton()
        {
            PlaySound(AudioType.ClickButtonSound);
        }

        public void PlaySound(AudioType sound, float volume = 0.1f, float pitch = 1f)
        {
            _soundsAudioSource.volume = volume;
            _soundsAudioSource.pitch = pitch;
            _soundsAudioSource.PlayOneShot(_sounds.GetSoundWithRandom(sound));
        }

        private IEnumerator ChangeMusic(AudioType music, int delay, float volume)
        {
            yield return new WaitForSeconds(delay);

            yield return MusicFade(0, () =>
            {
                _musicAudioSource.clip = _sounds.GetSoundWithRandom(music);
                _musicAudioSource.loop = true;
                _musicAudioSource.volume = 0.1f;
                _musicAudioSource.Play();
            }).WaitForKill();

            MusicFade(volume);
        }

        private Tween MusicFade(float endValue, Action onComplete = null)
        {
            var tween = DOTween.Sequence()
                .Append(DOTween.To(() => _musicAudioSource.volume, (x) => _musicAudioSource.volume = x, endValue, 1f))
                .OnKill(() => {
                    onComplete?.Invoke();
                });

            return tween;
        }

    }
}
 