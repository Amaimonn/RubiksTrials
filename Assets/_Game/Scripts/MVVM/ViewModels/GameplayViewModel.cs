using System;
using UnityEngine;
using R3;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.MVVM.Models;

namespace Assets._Game.Scripts.MVVM.ViewModels
{
    public class GameplayViewModel : ViewModel<GameplayModel>
    {
        public override Type ViewModelType => typeof(GameplayViewModel);
        public Observable<int> UndoCount => _undoCount;
        public Observable<bool> IsUndoAvailable => _isUndoAvailable;
        public Observable<int> RedoCount => _redoCount;
        public Observable<bool> IsRedoAvailable => _isRedoAvailable;
        public Observable<bool> IsExitWindowActive => _isExitWindowActive;
        
        private readonly ReactiveProperty<int> _undoCount = new();
        private readonly ReactiveProperty<int> _redoCount = new();
        private readonly ReactiveProperty<bool> _isUndoAvailable = new();
        private readonly ReactiveProperty<bool> _isRedoAvailable = new();
        private readonly ReactiveProperty<bool> _isExitWindowActive = new(false);

        private CompositeDisposable _disposables;


        public GameplayViewModel(GameplayModel model) : base(model)
        {
        }

        protected override void OnBind(GameplayModel model)
        {
            _disposables = new()
            {
                model.UndoCount.Subscribe(SetUndoCount),
                model.RedoCount.Subscribe(SetRedoCount),
                model.IsExitWindowActive.Subscribe(e => _isExitWindowActive.Value = e)
            };
        }

        public void OpenSettings()
        {
            _model.OpenSettings();
        }

        public void SetExitWindowActive(bool isActive)
        {
            _model.SetExitWindowActive(isActive);
        }
        
        public void OnExitButtonClicked()
        {
            _model.ExitLevel();
            _isExitWindowActive.Value = false;
            Dispose();
        }

        public void OnRotateXPositiveButtonClicked()
        {
            _model.Rotate(Vector3.right, true);
        }

        public void OnRotateXNegativeButtonClicked()
        {
            _model.Rotate(Vector3.right, false);
        }

        public void OnRotateYPositiveButtonClicked()
        {
            _model.Rotate(Vector3.up, true);
        }

        public void OnRotateYNegativeButtonClicked()
        {
            _model.Rotate(Vector3.up, false);
        }

        public void OnRotateZPositiveButtonClicked()
        {
            _model.Rotate(Vector3.forward, true);
        }

        public void OnRotateZNegativeButtonClicked()
        {
            _model.Rotate(Vector3.forward, false);
        }

        public void OnUndoButtonClicked()
        {
            _model.Undo();
        }

        public void OnRedoButtonClicked()
        {
            _model.Redo();
        }

        private void SetUndoCount(int count)
        {
            _undoCount.Value = count;
            _isUndoAvailable.Value = count > 0;
        }

        private void SetRedoCount(int count)
        {
            _redoCount.Value = count;
            _isRedoAvailable.Value = count > 0;
        }

        public override void Dispose()
        {
            _disposables.Dispose();
        }
    }
}