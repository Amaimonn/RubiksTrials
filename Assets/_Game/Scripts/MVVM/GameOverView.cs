using System;
using System.ComponentModel.DataAnnotations;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using R3;

public class GameOverView : MonoBehaviour
{
    [SerializeField, Required] private TMP_Text _visibleText;
    [SerializeField, Required] private Button _closeButton;
    [SerializeField] float _delayBetweenCharacters = 0.2f;
    
    private string _currentText;
    private Action _closeCallback;

    public void Show(string text, Action closeCallback)
    {
        Debug.Log($"popup:{text}"); 

        _closeCallback = closeCallback;
        _closeButton.gameObject.SetActive(false);
        _closeButton.OnClickAsObservable().Subscribe((e) => Close());

        ClearText();
        _currentText = text;
        gameObject.SetActive(true);

        if(text.Length > 0)
        {
            // StartCoroutine(ShowText(() => _closeButton.gameObject.SetActive(true)));

            Observable.Interval(TimeSpan.FromSeconds(_delayBetweenCharacters))
                .TakeWhile((e) => _visibleText.text.Length < _currentText.Length)
                .Subscribe(_ => ShowNextCharacter(), (e) => 
                {
                    Debug.Log("ok button active");
                    _closeButton.gameObject.SetActive(true);
                });
        }
        else
        {
            _closeButton.gameObject.SetActive(true);
        }
    }
    
    private void ClearText()
    {
        _visibleText.SetText("");
    }

    private void ShowNextCharacter()
    {
        _visibleText.SetText(_visibleText.text +_currentText[_visibleText.text.Length]);
        // if(_visibleText.text.Length >= _currentText.Length || _visibleText.text.Length >= 100)
        // {
        //     // _textDisposable.Dispose();
        //     _closeButton.gameObject.SetActive(true);
        // }
    }

    // private IEnumerator ShowText(Action onComplete = null)
    // {
    //     while (_visibleText.text.Length < _currentText.Length)
    //     {
    //         yield return new WaitForSeconds(_delayBetweenCharacters);
    //         _visibleText.SetText(_visibleText.text +_currentText[_visibleText.text.Length]);
    //     }
    //     onComplete?.Invoke();
    // }

    public void Close()
    {
        gameObject.SetActive(false);
        _closeCallback?.Invoke();
    }

    // private IEnumerator AppearAnimation()
    // {
    //     yield return new WaitForSeconds(1);
    //     _closeButton.gameObject.SetActive(true);
    // }

    // private IEnumerator ShowRoutine()
    // {
    //     var initialPosition = transform.localPosition;
    //     var initialScale = transform.localScale;
    //     var fade = StartCoroutine(FadeRoutine(1.0f, 0.0f, 2.0f));
    //     var move = StartCoroutine(MoveRoutine(initialPosition, initialPosition + Vector3.up * 200.0f, 0.2f));
    //     var scale = StartCoroutine(ScaleRoutine(initialScale, Vector3.zero, 0.2f));
    //     yield return fade;
    //     yield return move;
    //     yield return scale;
    //     gameObject.SetActive(false);
    //     transform.localPosition = initialPosition;
    //     transform.localScale = initialScale;
    //     _visibleText.alpha = 1.0f;
    // }

    // private IEnumerator FadeRoutine(float startFade = 1.0f, float endFade = 0.0f, float speed = 2.0f)
    // {
    //     yield return new WaitForSeconds(4.0f);
    //     var progress = 0.0f;
    //     while (progress < 1.0f)
    //     {
    //         _visibleText.alpha = Mathf.Lerp(startFade, endFade, progress);
    //         yield return null; 
    //         progress += Time.deltaTime * speed;
    //     }
    // }

    // private IEnumerator MoveRoutine(Vector3 startPosition, Vector3 endPosition, float speed = 3.0f)
    // {
    //     var progress = 0.0f;
    //     while (progress < 1.0f)
    //     {
    //         transform.localPosition = Vector3.Lerp(startPosition, endPosition, progress);
    //         yield return null; 
    //         progress += Time.deltaTime * speed;
    //     }
    // }

    // private IEnumerator ScaleRoutine(Vector3 startScale, Vector3 endScale, float speed)
    // {
    //     yield return new WaitForSeconds(1.0f);
    //     var progress = 0.0f;
    //     transform.localScale = startScale;
    //     while (progress < 1.0f)
    //     {
    //         transform.localScale = Vector3.Lerp(startScale, endScale, progress);
    //         yield return null; 
    //         progress += Time.deltaTime * speed;
    //     }
    // }

}
