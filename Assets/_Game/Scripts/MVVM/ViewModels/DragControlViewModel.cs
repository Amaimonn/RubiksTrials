using System;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.MVVM.Models;
using R3;
using UnityEngine;

namespace Assets._Game.Scripts.MVVM.ViewModels
{
    public class DragControlViewModel : ViewModel<GameplayModel>
    {
        public override Type ViewModelType => typeof(DragControlViewModel);
        public Observable<bool> IsControlEnabled => _isControlEnabled;

        private ReactiveProperty<bool> _isControlEnabled = new();
        private CompositeDisposable _disposables;

        public DragControlViewModel(GameplayModel model) : base(model)
        {
        }

        protected override void OnBind(GameplayModel model)
        {
            _disposables = new()
            {
                model.IsControlsEnabled.Subscribe(e => _isControlEnabled.Value = e)
            };
        }

        public void Swipe(Vector3 partNormal, Vector3 swipeDirection, Vector3 partPosition)
        {
            var plane = new Plane(partNormal + partPosition, swipeDirection + partPosition, partPosition);
            var definePartsCommand = new GetPartsOnPlaneCommand(plane); // to model
            _model.Swipe(definePartsCommand, plane.normal, true);
        }

        public override void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}