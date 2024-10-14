using System.Collections;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Vector3 _amplitudeTransform;
    [SerializeField] private AnimationCurve _movementCurve;
    [SerializeField] private float _speedMultiplier = 1;
    private Vector3 _initialRectTransform;

    private void OnEnable()
    {
        _initialRectTransform = _rectTransform.localPosition;
        StartCoroutine(Animate());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        _rectTransform.localPosition = _initialRectTransform;
    }

    private IEnumerator Animate()
    {
        float progress;
        float timer;
        
        while(true)
        {
            timer = 0;
            while (timer < _movementCurve.keys[_movementCurve.length - 1].time)
            {
                progress = _movementCurve.Evaluate(timer);
                _rectTransform.localPosition = (1 - progress) * _initialRectTransform + progress * _amplitudeTransform;
                timer += Time.deltaTime * _speedMultiplier;
                yield return null;
            }
        }
    }
}
