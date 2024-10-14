using System;
using System.Collections;
using UnityEngine;

namespace Assets._Game.Scripts.MVVM.Models
{
    public class LimitedSwipesLevel : TimeProgressLevel
    {
        private int _currentMinutes = 1;
        private int _currentSeconds = 0;
        private int _currentLevel = 0;
        // private readonly Action<int> _onNextStage;

        public LimitedSwipesLevel(CommonRubiksCube rubiksCube, Action onLevelExit, Action refresh) : 
            base(rubiksCube, onLevelExit)
        {
            OnRefresh = refresh;
            //_onNextStage = e => _rubiksCube.MixCube(e, _rubiksCube.CheckIsSolved);// onNextStage;
            UpdateLevelText();
            UpdateTimeText();
        }

        public override IEnumerator StartLevel()
        {
            yield return _rubiksCube.MixCube(1, _rubiksCube.CheckIsSolved);
            yield return new WaitForSeconds(0.5f);
            StartTimer();
        }

        public override void TryComplete()
        { 
            if (_rubiksCube.CheckIsSolved())
            {
                _cts.Cancel();
                OnRefresh();
                _currentLevel++;
                UpdateLevelText();

                // _onNextStage.Invoke((int)Mathf.Sqrt(_currentLevel) + 1);
                OnComplete?.OnNext(true);
                Debug.Log("Level completed");
                _rubiksCube.StartCoroutine(_rubiksCube.MixCube((int)Mathf.Sqrt(_currentLevel) + 1, 
                    _rubiksCube.CheckIsSolved, 
                    onFinishCallback: () => {
                        _currentMinutes = 1;
                        _currentSeconds = 0;
                        UpdateTimeText();
                        StartTimer();
                    }
                ));
                
            }
        }

        public override void Finish()
        {
            _cts.Cancel();
            // _timeText.OnNext($" Уровней пройдено: {_currentLevel}");
            bool isSuccess = _currentLevel > 0;
            OnComplete?.OnNext(isSuccess);
            
            var gameOverViewParameters = new GameOverViewParameters($" Уровней пройдено: {_currentLevel}", 
                () => OnLevelExit?.OnNext(isSuccess));
            _resultsWindow?.OnNext(gameOverViewParameters);
            // OnLevelExit?.OnNext(isSuccess);
        }

        // public override IEnumerator StartTimer()
        // {
            // while (_currentMinutes + _currentSeconds > 0)
            // {
            //     _timeText.OnNext(string.Format("{0:00}:{1:00}:{2:00}",0,_currentMinutes,_currentSeconds));
            //     yield return new WaitForSeconds(1.0f);
            //     _currentSeconds--;

            //     if (_currentSeconds < 0)
            //     {
            //         if (_currentMinutes > 0)
            //         {
            //             _currentSeconds = 59;
            //             _currentMinutes--;
            //         }
            //     }
            // }
            // _timeText.OnNext(string.Format("{0:00}:{1:00}:{2:00}",0,_currentMinutes,_currentSeconds));
            // _textAnimation.OnNext(t => {
            //     OnRefresh();
            //     _ending.OnNext(false);
            //     return TextBounce(t, Exit);
            // });
        //}

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
            _progressText.Value = $"Уровень: {_currentLevel + 1}";
        }
    }
}