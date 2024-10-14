using System;
using System.Collections;
using R3;
using Assets._Game.Scripts.MVVM.Abstractions;

namespace Assets._Game.Scripts.MVVM.Models
{
    public abstract class AbstractLevelModel : IModel
    {
        public virtual Type LevelType => typeof(AbstractLevelModel);
        public Observable<bool> IsInputEnabled => _isInputEnabled;
        public Subject<bool> OnLevelExit = new();
        public Subject<bool> OnComplete = new();
        public Observable<GameOverViewParameters> ResultsWindow => _resultsWindow;
        
        public Action OnRefresh;

        protected ReactiveProperty<bool> _isInputEnabled = new(true);
        protected Subject<GameOverViewParameters> _resultsWindow = new();
        protected CompositeDisposable _disposables;
        protected CommonRubiksCube _rubiksCube;

        public AbstractLevelModel(CommonRubiksCube rubiksCube, Action onLevelExit)
        {
            _disposables = new()
            {
                OnLevelExit.Subscribe((e) => onLevelExit?.Invoke())
            };
            _rubiksCube = rubiksCube;
        }

        public virtual IEnumerator StartLevel()
        {
            yield return null;
        }

        public abstract void TryComplete();

        public abstract void Finish();

        public virtual void Dispose()
        {
            _disposables.Dispose();
        }
    }
}