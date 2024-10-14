using System;
using System.Collections;
using UnityEngine;

namespace Assets._Game.Scripts.MVVM.Models
{
    public class CollectColorsLevel : TimeProgressLevel
    {
        private int _currentMinutes = 5;
        private int _currentSeconds = 0;
        private int _collectedColorsCount = 0;
        private readonly Func<int> _getSolvedFacesCount;

        public CollectColorsLevel(CommonRubiksCube rubiksCube, Action onLevelExit, Action refresh) : 
            base(rubiksCube, onLevelExit)
        {
            OnRefresh = refresh;
            _getSolvedFacesCount = rubiksCube.GetSolvedFacesCount;
            UpdateLevelText();
            UpdateTimeText();
        }

        public override IEnumerator StartLevel()
        {
            yield return _rubiksCube.MixCube(25, _rubiksCube.CheckAnyFaceIsSolved);
            yield return new WaitForSeconds(0.5f);
            StartTimer();
        }

        public override void TryComplete()
        {
            if (_rubiksCube.CheckAnyFaceIsSolved())
            {
                _cts.Cancel();
                OnRefresh();
                _collectedColorsCount += _getSolvedFacesCount();
                Debug.Log("Получено цветов: " + _getSolvedFacesCount());
                UpdateLevelText();
                OnComplete?.OnNext(true);
                _rubiksCube.StartCoroutine( _rubiksCube.MixCube(15, 
                    _rubiksCube.CheckAnyFaceIsSolved,
                    1600.0f, 
                    () => { StartTimer(); }
                ));
            }
        }

        public override void Finish()
        {
            _cts.Cancel();
            bool isSuccess = _collectedColorsCount > 0;
            OnComplete?.OnNext(isSuccess);

            var gameOverViewParameters = new GameOverViewParameters($" Цветов собрано: {_collectedColorsCount}", 
                () => OnLevelExit?.OnNext(isSuccess));
            _resultsWindow?.OnNext(gameOverViewParameters);
        }

        protected override void UpdateTime()
        {
            if (_currentMinutes + _currentSeconds > 0)
            {
                _currentSeconds--;

                if (_currentSeconds < 0)
                {
                    if (_currentMinutes > 0)
                    {
                        _currentSeconds = 59;
                        _currentMinutes--;
                    }
                }
                UpdateTimeText();
            }
            else
            {
                _cts.Cancel();
                UpdateTimeText();
                _textAnimation.OnNext(t => {
                    OnRefresh();
                    _isInputEnabled.Value = false;
                    return TextBounce(t, Finish);
                });
            }
        }

        protected override void UpdateTimeText()
        {
            _timeText.Value = string.Format("{0:00}:{1:00}:{2:00}",0,_currentMinutes,_currentSeconds);
        }
        
        private void UpdateLevelText()
        {
            _progressText.Value = $"Цветов собрано: {_collectedColorsCount}";
        }
    }
}