using System.Collections;
using UnityEngine;
using Assets._Game.Scripts.MVVM;

public class ExitPopUp : DecisionPopUp
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Vector3 _startPosition = new (0, 170, 0);
    [SerializeField] private Vector3 _endPosition = Vector3.zero;
    [SerializeField] private float _scaleDuration = 0.6f;
    [SerializeField] private float _moveDuration = 0.5f;

    public override void Show()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            StartCoroutine(ShowRoutine());
        }
    }

    public override void Close()
    {
        if (gameObject.activeSelf)
            StartCoroutine(CloseRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        var scaleRoutine = StartCoroutine(ScaleRoutine(new Vector3(0.5f, 0.5f, 0.5f), Vector3.one, _scaleDuration));
        var moveRoutine = StartCoroutine(MoveRoutine(_startPosition, _endPosition, _moveDuration));
        yield return scaleRoutine;
        yield return moveRoutine;
       _isInputEnabled = true;
    }

    private IEnumerator CloseRoutine()
    {
        _isInputEnabled = false;
        var scaleRoutine = ScaleRoutine(Vector3.one, new Vector3(0.7f, 0.7f, 0.7f), _scaleDuration / 2);
        var moveRoutine = StartCoroutine(MoveRoutine(_endPosition, _startPosition, _moveDuration / 2));
        yield return scaleRoutine;
        yield return moveRoutine;
        gameObject.SetActive(false);
    }

    private IEnumerator ScaleRoutine(Vector3 startScale, Vector3 endScale, float duration)
    {
        var eclapsedTime = 0.0f;
        _panel.transform.localScale = startScale;
        while (eclapsedTime < duration)
        {
            _panel.transform.localScale = Vector3.Lerp(startScale, endScale, eclapsedTime / duration);
            yield return null; 
            eclapsedTime += Time.deltaTime;
        }
    }

    private IEnumerator MoveRoutine(Vector3 startPosition, Vector3 endPosition, float duration)
    {
        var eclapsedTime = 0.0f;
        _panel.transform.localPosition = startPosition;
        while (eclapsedTime < duration)
        {
            _panel.transform.localPosition = Vector3.Lerp(startPosition, endPosition, eclapsedTime / duration);
            yield return null;
            eclapsedTime += Time.deltaTime;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
