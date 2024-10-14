using System;
using System.Collections;
using System.Threading;
using R3;
using TMPro;
using UnityEngine;

namespace Assets._Game.Scripts.MVVM.Models
{
    public abstract class TimeProgressLevel : AbstractLevelModel
    {
        public override Type LevelType => typeof(TimeProgressLevel);
        public Observable<string> TimeText => _timeText;
        public Observable<string> ProgressText => _progressText;
        public Observable<Func<TMP_Text, IEnumerator>> TextAnimation => _textAnimation;
        

        protected ReactiveProperty<string> _timeText = new();
        protected ReactiveProperty<string> _progressText = new();
        protected Subject<Func<TMP_Text, IEnumerator>> _textAnimation = new();
        

        protected CancellationTokenSource _cts = new();

        public TimeProgressLevel(CommonRubiksCube rubiksCube, Action onLevelExit) : base(rubiksCube, onLevelExit)
        {
        }

        protected virtual  void StartTimer()
        {
            _cts = new CancellationTokenSource();
            Observable.Interval(TimeSpan.FromSeconds(1.0f)).TakeUntil(_cts.Token).Subscribe(_ => UpdateTime());
        }

        protected abstract  void UpdateTime();
        protected abstract void UpdateTimeText();

        protected virtual IEnumerator TextBounce(TMP_Text animatedText, Action OnCompleteCallback = null)
        {
            float animationTime;
            var growTime = 0.9f;
            var shrinkTime = 0.9f;

            for (var i = 0; i < 3; i++)
            {
                animationTime = 0f;
                while (animationTime < growTime)
                {
                    animatedText.rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f , animationTime / growTime);
                    animationTime += Time.deltaTime;
                    yield return null;
                }

                animationTime = 0f;
                while (animationTime < shrinkTime)
                {
                    animatedText.rectTransform.localScale = Vector3.Lerp(Vector3.one * 1.2f, Vector3.one, animationTime / shrinkTime);
                    animationTime += Time.deltaTime;
                    yield return null;
                }
            }

            animatedText.rectTransform.localScale = Vector3.one;
            OnCompleteCallback?.Invoke();
        }
    }
}