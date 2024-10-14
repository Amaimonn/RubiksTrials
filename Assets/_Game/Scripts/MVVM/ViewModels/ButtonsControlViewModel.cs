using System;
using UnityEngine;
using R3;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.MVVM.Models;

namespace Assets._Game.Scripts.MVVM.ViewModels
{
    public class ButtonsControlViewModel : ViewModel<GameplayModel>
    {
        public override Type ViewModelType => typeof(ButtonsControlViewModel);
        public Observable<bool> IsControlEnabled => _isControlEnabled;

        private ReactiveProperty<bool> _isControlEnabled = new();
        private Func<Vector3, IGetPartsCommand> GetPartsCommand => (e) => new GetPartsWithAxisCommand(e);
        private CompositeDisposable _disposables;


        public ButtonsControlViewModel(GameplayModel model) : base(model)
        {
        }

        protected override void OnBind(GameplayModel model)
        {
            _disposables = new()
            {
                model.IsControlsEnabled.Subscribe(e => _isControlEnabled.Value = e)
            };
        }

        public void OnSwipeNorthPositiveButtonClicked()
        {
            _model.Swipe(GetPartsCommand(Vector3.forward), Vector3.forward, true);
        }

        public void OnSwipeNorthNegativeButtonClicked()
        {
            _model.Swipe(GetPartsCommand(Vector3.forward), Vector3.forward, false);
        }

        public void OnSwipeSouthPositiveButtonClicked()
        {
            _model.Swipe(GetPartsCommand(Vector3.back), Vector3.back, true);
        }

        public void OnSwipeSouthNegativeButtonClicked()
        {
            _model.Swipe(GetPartsCommand(Vector3.back), Vector3.back, false);
        }

        public void OnSwipeEastPositiveButtonClicked()
        {
            _model.Swipe(GetPartsCommand(Vector3.right), Vector3.right, true);
        }

        public void OnSwipeEastNegativeButtonClicked()
        {
            _model.Swipe(GetPartsCommand(Vector3.right), Vector3.right, false);
        }

        public void OnSwipeWestPositiveButtonClicked()
        {
            _model.Swipe(GetPartsCommand(Vector3.left), Vector3.left, true);
        }

        public void OnSwipeWestNegativeButtonClicked()
        {
            _model.Swipe(GetPartsCommand(Vector3.left), Vector3.left, false);
        }

        public void OnSwipeTopPositiveButtonClicked()
        {
            _model.Swipe(GetPartsCommand(Vector3.up), Vector3.up, true);
        }

        public void OnSwipeTopNegativeButtonClicked()
        {
            _model.Swipe(GetPartsCommand(Vector3.up), Vector3.up, false);
        }

        public void OnSwipeBottomPositiveButtonClicked()
        {
            _model.Swipe(GetPartsCommand(Vector3.down), Vector3.down, true);
        }

        public void OnSwipeBottomNegativeButtonClicked()
        {
            _model.Swipe(GetPartsCommand(Vector3.down), Vector3.down, false);
        }

        public override void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}