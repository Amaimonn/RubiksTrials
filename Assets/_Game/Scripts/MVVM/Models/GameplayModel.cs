using System;
using System.Linq;
using UnityEngine;
using R3;
using ObservableCollections;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.Processors;

namespace Assets._Game.Scripts.MVVM.Models
{
    public class GameplayModel : IModel, IDisposable
    {
        public Observable<bool> IsControlsEnabled => _isControlsEnabled;
        public Observable<int> UndoCount => _undoCount;
        public Observable<int> RedoCount => _redoCount;
        public Observable<bool> IsExitWindowActive => _isExitWindowActive;
        public event Action OnSwiped;

        private readonly ReactiveProperty<int> _undoCount = new();
        private readonly ReactiveProperty<int> _redoCount = new();
        private readonly ReactiveProperty<bool> _isExitWindowActive = new(false);
        private ReadOnlyReactiveProperty<bool> _isControlsEnabled;
        private readonly CommonRubiksCube _mainCube;
        private readonly int _sidePartsCount;
        private readonly IControlsProcessor _controller;
        private readonly AbstractLevelModel _level;
        private readonly SettingsModel _settingsModel;
        private float RotationAngle => _mainCube.RotationAngle;
        private readonly CompositeDisposable _disposables;

        public GameplayModel(CommonRubiksCube mainCube, IControlsProcessor controller, Invoker invoker, AbstractLevelModel level, SettingsModel settingsModel, 
            Action backToMenu)
        {
            _mainCube = mainCube;
            _sidePartsCount = (int)Math.Round(Math.Pow(_mainCube.CubeParts.Length, 2.0f/3));
            _controller = controller;
            _controller.OnSwiped += level.TryComplete;
            _controller.OnSwiped += OnSwiped;
            _level = level;
            _settingsModel = settingsModel;
            _disposables = new()
            {
                invoker.UndoLongCommandsBuffer.ObserveCountChanged().Subscribe(OnUndoCountChangedHandler),
                invoker.RedoLongCommandsBuffer.ObserveCountChanged().Subscribe(OnRedoCountChangedHandler),
                _level.OnLevelExit.Subscribe((e) =>
                {
                    backToMenu();
                    Dispose();
                })
            
            };

            // _isControlsEnabled = Observable.Merge(_settingsModel.IsOpened, _isExitWindowActive, _level.IsInputEnabled.Select(x => !x))
            //     .Select(x => !x)
            //     .ToReadOnlyReactiveProperty();

            _isControlsEnabled = _isExitWindowActive.Select(x => !x)
                .CombineLatest(_settingsModel.IsOpened.Select(x => !x), _level.IsInputEnabled, (a, b, c) => a && b && c)
                .ToReadOnlyReactiveProperty();
        }

        public void OpenSettings()
        {
            _settingsModel.SetSettingsWindowActive(true);
        }

        public void SetExitWindowActive(bool isActive)
        {
            _isExitWindowActive.Value = isActive;
        }
        
        public void OnUndoCountChangedHandler(int undoCount)
        {
            _undoCount.Value = undoCount;
        }

        public void OnRedoCountChangedHandler(int redoCount)
        {
            _redoCount.Value = redoCount;
        }

        public void ExitLevel()
        {
            _level.Finish();
        }

#region controls
        public void Swipe(IGetPartsCommand partsCommand, Vector3 axis, bool isClockwise)
        {
            var sign = isClockwise ? 1 : -1;
            var partsToSwipe = partsCommand.GetParts(_mainCube.CubeParts);
            if (partsToSwipe.Count() == _sidePartsCount)
            {
                _controller.Swipe(partsToSwipe, axis, sign * RotationAngle);
            }
            else
            {
                Debug.Log($"partsToSwipe: {partsToSwipe.Count()}, need: {_sidePartsCount}");
            }
        }

        public void Rotate(Vector3 axis, bool isClockwise)
        {
            var sign = isClockwise ? 1 : -1;
            _controller.Rotate(axis, sign * RotationAngle);
        }

        public void Undo()
        {
            _controller.Undo();
        }

        public void Redo()
        {
            _controller.Redo();
        }
#endregion

        public void Dispose()
        {
            _controller.OnSwiped -= _level.TryComplete;
            _controller.OnSwiped -= OnSwiped;
            _disposables.Dispose();
        }
    }
}