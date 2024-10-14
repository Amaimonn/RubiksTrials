using System;
using R3;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.MVVM.Models;

namespace Assets._Game.Scripts.MVVM.ViewModels
{
    public class TutorialLevelViewModel : ViewModel<TutorialLevel>
    {
        public override Type ViewModelType => typeof(TutorialLevelViewModel);
        public int StagesCount => _stagesCount;
        public Observable<int> StageIndex => _stageIndex;
        public Observable<bool> IsNextStageEnabled => _isNextStageEnabled;
        public Observable<bool> IsExitEnabled => _isExitEnabled;
        public Observable<bool> IsHelpMenuOpened => _isHelpMenuOpened;
        public Observable<GameOverViewParameters> ResultsWindow => _resultsWindow;

        protected ReactiveProperty<int> _stageIndex = new ();
        protected ReactiveProperty<bool> _isNextStageEnabled = new ();
        protected ReactiveProperty<bool> _isExitEnabled = new ();
        protected ReactiveProperty<bool> _isHelpMenuOpened = new (false);
        protected Subject<GameOverViewParameters> _resultsWindow = new();

        protected int _stagesCount;
        protected CompositeDisposable _disposables;
        public TutorialLevelViewModel(TutorialLevel model) : base(model)
        {
        }

        protected override void OnBind(TutorialLevel model)
        {
            _disposables = new()
            {
                model.StageIndex.Subscribe(SetStageIndex),
                model.IsNextStageEnabled.Subscribe(SetIsNextStageEnabled),
                model.IsExitEnabled.Subscribe(SetIsExitEnabled),
                model.ResultsWindow.Subscribe((e) => _resultsWindow.OnNext(e)),
                model.OnLevelExit.Subscribe((e) => Unbind())
            };

            _stagesCount = model.StagesCount;
        }

        public void OnRestartStageButtonClicked()
        {
            _model.RestartStage();
        }

        public void OnSetStageButtonClicked(int stageIndex)
        {
            _model.SetStage(stageIndex);
        }

        public void OnNextStageButtonClicked()
        {
            _model.SetStage(_stageIndex.Value + 1);
        }

        public void OnExitButtonClicked()
        {
            _model.Finish();
        }

        public void OnHelpButtonClicked()
        {
            _model.OpenHelpMenu();
        }

        private void SetStageIndex(int index)
        {
            _stageIndex.Value = index;
        }

        private void SetIsNextStageEnabled(bool isEnabled)
        {
            _isNextStageEnabled.Value = isEnabled;
        }

        private void SetIsExitEnabled(bool isEnabled)
        {
            _isExitEnabled.Value = isEnabled;
        }

        private void Unbind()
        {
            ServiceLocator.Current.Get<UIManager>().Hide<TutorialLevelViewModel>();
            Dispose();
        }

        public override void Dispose()
        {
            _disposables.Dispose();
        }
    }
}