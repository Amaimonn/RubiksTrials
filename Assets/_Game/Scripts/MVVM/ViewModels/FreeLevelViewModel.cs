using System;
using UnityEngine;
using R3;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.MVVM.Models;

namespace Assets._Game.Scripts.MVVM.ViewModels
{
    public class FreeLevelViewModel : ViewModel<FreeLevel>
    {
        public override Type ViewModelType => typeof(FreeLevelViewModel);
        public Observable<bool> OnLevelExit => _onLevelExit;
        public Observable<GameOverViewParameters> ResultsWindow => _resultsWindow;

        protected Subject<bool> _onLevelExit = new ();
        protected Subject<GameOverViewParameters> _resultsWindow = new ();
        protected CompositeDisposable _disposables;

        public FreeLevelViewModel(FreeLevel model) : base(model)
        {
        }

        protected override void OnBind(FreeLevel model)
        {
            _disposables = new()
            {
                model.OnLevelExit.Subscribe((e) => {
                    _onLevelExit.OnNext(e);
                    Unbind();
                }),
                model.ResultsWindow.Subscribe((callback) => _resultsWindow.OnNext(callback))
            };
        }

        public void OnRefreshPazzleButtonClicked()
        {
            Debug.Log("Refresh Pazzle");
            _model.RefreshPazzle();
        }

        public void OnMixPazzleButtonClicked()
        {
            Debug.Log("Mix Pazzle");
            _model.MixPazzle();
        }

        private void Unbind()
        {
            ServiceLocator.Current.Get<UIManager>().Hide<FreeLevelViewModel>();
            Dispose();
        }

        public override void Dispose()
        {
            _disposables.Dispose();
        }
    }
}