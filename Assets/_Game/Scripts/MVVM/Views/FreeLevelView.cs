using UnityEngine;
using UnityEngine.UI;
using R3;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.MVVM.ViewModels;

namespace Assets._Game.MVVM.Views
{
    public class FreeLevelView : BaseScreen<FreeLevelViewModel>
    {
        [SerializeField] private Button _refreshButton;
        [SerializeField] private Button _mixButton;
        [SerializeField] private GameOverView _gameOverPopUpText;
        // [SerializeField] private string _gameOverText = "Удачи ^-^";

        private CompositeDisposable _disposables;
        
        protected override void OnBind(FreeLevelViewModel viewModel)
        {
            _refreshButton.onClick.AddListener(viewModel.OnRefreshPazzleButtonClicked);
            _mixButton.onClick.AddListener(viewModel.OnMixPazzleButtonClicked);

            _disposables = new()
            {
                viewModel.ResultsWindow.Subscribe((parameters) => _gameOverPopUpText.Show(parameters.Message, parameters.ExitCallback)),
            };
        }

        public override void Dispose()
        {
            _refreshButton.onClick.RemoveAllListeners();
            _mixButton.onClick.RemoveAllListeners();
            _disposables.Dispose();
        }
    }
}