using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip _doubleUp;
    [SerializeField] private AudioClip _doubleDown;
    [SerializeField] private AudioClip _sonorous;
    [SerializeField] private AudioClip _oneTap;
    [SerializeField] private float _sfxVolume = 1.0f;

    public void PlayDoubleUp()
    {
        audioSource.PlayOneShot(_doubleUp, _sfxVolume);
    }

    public void PlayDoubleDown()
    {
        audioSource.PlayOneShot(_doubleDown, _sfxVolume);
    }

    public void PlaySonorous()
    {
        audioSource.PlayOneShot(_sonorous, _sfxVolume);
    }

    public void PlayOneTap()
    {
        audioSource.PlayOneShot(_oneTap, _sfxVolume);
    }
}
