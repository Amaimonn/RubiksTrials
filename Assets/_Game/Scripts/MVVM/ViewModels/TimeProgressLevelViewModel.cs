using System;
using System.Collections;
using TMPro;
using R3;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.MVVM.Models;

namespace Assets._Game.Scripts.MVVM.ViewModels
{
    public class TimeProgressLevelViewModel : ViewModel<TimeProgressLevel>
    {
        public override Type ViewModelType => typeof(TimeProgressLevelViewModel);
        public Observable<string> TimeText => _timeText;
        public Observable<string> ProgressText => _progressText;
        public Observable<Func<TMP_Text, IEnumerator>> TextAnimation => _textAnimation;
        public Observable<bool> OnLevelExit => _onLevelExit;
        public Observable<bool> Ending => _ending;
        public Observable<GameOverViewParameters> ResultsWindow => _resultsWindow;

        protected ReactiveProperty<string> _timeText = new();
        protected ReactiveProperty<string> _progressText = new();
        protected Subject<Func<TMP_Text, IEnumerator>> _textAnimation = new();
        protected Subject<bool> _onLevelExit = new();
        protected Subject<bool> _ending = new();
        protected Subject<GameOverViewParameters> _resultsWindow = new();
        private CompositeDisposable _disposables;

        public TimeProgressLevelViewModel(TimeProgressLevel model) : base(model)
        {
        }

        protected override void OnBind(TimeProgressLevel model)
        {
            _disposables = new()
            {
                model.TimeText.Subscribe((e) => _timeText.Value = e),
                model.ProgressText.Subscribe((e) => _progressText.OnNext(e)),
                model.TextAnimation.Subscribe((e) => _textAnimation.OnNext(e)),
                model.IsInputEnabled.Subscribe((e) => _ending.OnNext(e)),
                model.ResultsWindow.Subscribe((callback) => _resultsWindow.OnNext(callback)),
                model.OnLevelExit.Subscribe((e) => {
                    _onLevelExit.OnNext(e);
                    Unbind();
                })
            };
        }

        protected void Unbind()
        {
            ServiceLocator.Current.Get<UIManager>().Hide<TimeProgressLevelViewModel>();
            Dispose();
        }

        public override void Dispose()
        {
            _disposables.Dispose();
        }
    }
}