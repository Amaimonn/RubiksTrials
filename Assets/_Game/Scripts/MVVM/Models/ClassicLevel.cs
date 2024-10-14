using System;
using System.Collections;
using UnityEngine;

namespace Assets._Game.Scripts.MVVM.Models
{
    public class ClassicLevel : TimeProgressLevel
    {    
        public override Type LevelType => typeof(ClassicLevel);

        protected int _currentHours = 0;
        protected int _currentMinutes = 0;
        protected int _currentSeconds = 0;

        protected bool _isSuccess = false;

        public ClassicLevel(CommonRubiksCube rubiksCube, Action onLevelExit, Action refresh) : base(rubiksCube, onLevelExit)
        {
            OnRefresh = refresh;
            UpdateTimeText();
        }

        public override IEnumerator StartLevel()
        {
            yield return _rubiksCube.MixCube(25, _rubiksCube.CheckIsSolved);
            yield return new WaitForSeconds(0.5f);
            StartTimer();
        }

        public override void TryComplete()
        {
            if (_rubiksCube.CheckIsSolved())
            {
                _isSuccess = true;
                Finish();
            }
        }

        public override void Finish()
        {
            _cts.Cancel();
            OnComplete?.OnNext(_isSuccess);
            OnLevelExit?.OnNext(_isSuccess);

            var gameOverViewParameters = new GameOverViewParameters($" Прошло времени: {_timeText.Value}", 
                () => OnLevelExit?.OnNext(_isSuccess));
            _resultsWindow?.OnNext(gameOverViewParameters);
        }

        protected override void UpdateTime()
        {
            _currentSeconds++;

            if (_currentSeconds == 60)
            {
                _currentSeconds = 0;
                _currentMinutes++;
                if (_currentMinutes == 60)
                {
                    _currentMinutes = 0;
                    _currentHours++;
                }
            }
            UpdateTimeText();
        }

        protected override void UpdateTimeText()
        {
            _timeText.Value = string.Format("{0:00}:{1:00}:{2:00}",_currentHours,_currentMinutes,_currentSeconds);
        }
    }
}