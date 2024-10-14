using UnityEngine;
using TMPro;
using R3;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.MVVM.ViewModels;

namespace Assets._Game.Scripts.MVVM.Views
{
    public class TimeProgressLevelView : BaseScreen<TimeProgressLevelViewModel>
    {
        [SerializeField] private TMP_Text _timeText;
        [SerializeField] private TMP_Text _progressText;
        [SerializeField] private GameObject _fadeImage;
        [SerializeField] private GameOverView _gameOverPopUpText;

        private CompositeDisposable _disposables;

        protected override void OnBind(TimeProgressLevelViewModel viewModel)
        {
            _disposables = new()
            {
                viewModel.TimeText.Subscribe((e) => _timeText.text = e),
                viewModel.ProgressText.Subscribe((e) => _progressText.text = e),
                viewModel.TextAnimation.Subscribe((routine) => StartCoroutine(routine(_timeText))),
                viewModel.ResultsWindow.Subscribe((parameters) => _gameOverPopUpText.Show(parameters.Message, parameters.ExitCallback)),
                viewModel.Ending.Subscribe((e) => FadeArea())
            };
        }

        public override void Show()
        {
            base.Show();
        }

        public override void Dispose()
        {
            _disposables.Dispose();
        }

        private void FadeArea()
        {
            _fadeImage.SetActive(true);
        }

        private void Awake()
        {
            _fadeImage.SetActive(false);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _fadeImage.SetActive(false);
        }
    }
}