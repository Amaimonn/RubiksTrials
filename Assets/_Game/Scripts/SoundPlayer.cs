using System.Collections;
using UnityEngine;

namespace Assets._Game.Scripts
{
    public class SoundPlayer : MonoBehaviour, IService
    {
        [Header("SFX")]
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioClip _swipeClip;
        [SerializeField] private AudioClip _rotationClip;
        [SerializeField] private AudioClip _victoryClip;
        [SerializeField] private AudioClip _lossClip;
        [SerializeField] private float _sfxVolumeScale = 1.0f;

        [Header("Sound")]
        [SerializeField] private AudioSource _soundSource;
        [SerializeField] private AudioClip _soundClip;
        [SerializeField] private float _musicDelay = 2.0f;
        [SerializeField] private float _fadeDuration = 2.5f;
        [SerializeField] private float _soundVolumeScale = 1.0f;

        public void SetSoundVolume(float volume)
        {
            _soundSource.volume = volume;
        }

        public void SetSFXVolume(float volume)
        {
            _sfxSource.volume = volume;
        }

        public void PlaySwipe()
        {
            _sfxSource.PlayOneShot(_swipeClip, _sfxVolumeScale);
        }

        public void PlayRotation()
        {
            _sfxSource.PlayOneShot(_rotationClip, _sfxVolumeScale);
        }

        public void PlayVictory()
        {
            _sfxSource.PlayOneShot(_victoryClip, _sfxVolumeScale);
        }

        public void PlayLoss()
        {
            _sfxSource.PlayOneShot(_lossClip, _sfxVolumeScale);
        }

        private void Awake()
        {
            _soundSource.clip = _soundClip;
        }

        private void OnEnable()
        {
            StartCoroutine(PlaySoundLoop());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator PlaySoundLoop()
        {
            while(gameObject.activeSelf)
            {
                StartCoroutine(FadeSoundVolume(0f, _soundVolumeScale, _fadeDuration));
                _soundSource.Play();
                yield return new WaitForSeconds(_soundClip.length - _fadeDuration);
                yield return StartCoroutine(StopSound());
                yield return new WaitForSeconds(_musicDelay);
            }
        }

        private IEnumerator StopSound()
        {
            StartCoroutine(FadeSoundVolume(_soundVolumeScale, 0f, _fadeDuration));
            yield return new WaitForSeconds(_fadeDuration);
            _soundSource.Stop();
        }

        private IEnumerator FadeSoundVolume(float fromValue, float toValue, float duration)
        {
            var time = 0f;
            while(time < duration)
            {
                _soundSource.volume = Mathf.Lerp(fromValue, toValue, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            _soundSource.volume = toValue;
        }
    }
}