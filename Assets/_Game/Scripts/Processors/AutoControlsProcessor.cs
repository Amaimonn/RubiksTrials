using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._Game.Scripts.Processors
{
    public class AutoControlsProcessor : IControlsProcessor
    {
        public event Action OnSwiped;
        private readonly Invoker _invoker;
        private readonly CommonRubiksCube _mainCube;
        private readonly IEnumerable<CubePart> _cubeParts;
        private readonly Action _playSwipeSound;
        private readonly Action _playRotateSound;

        public AutoControlsProcessor(Invoker invoker, CommonRubiksCube mainCube)
        {
            _invoker = invoker;
            _mainCube = mainCube;
            _cubeParts = _mainCube.GetComponentsInChildren<CubePart>();
            var soundPlayer = ServiceLocator.Current.Get<SoundPlayer>();
            _playSwipeSound = () => soundPlayer.PlaySwipe();
            _playRotateSound = () => soundPlayer.PlayRotation();
        }

        public void Rotate(Vector3 axis, float angle)
        {
            InvokeRotateCubeLongCommand(axis, angle);
        }

        public void Swipe(IEnumerable<CubePart> partsToSwipe, Vector3 axis, float angle)
        {
            InvokeSwipeLongCommand(partsToSwipe, axis, angle);
        }

        public void Undo()
        {
            if (_cubeParts.All(cube => cube.IsBusy == false))
            {
                // var currentPartsList = _cubeParts.ToList();
                // currentPartsList.ForEach(part => part.IsBusy = true);
                _invoker.UndoLastLongCommand();
            }
        }

        public void Redo()
        {
            if (_cubeParts.All(cube => cube.IsBusy == false))
            {
                // var currentPartsList = _cubeParts.ToList();
                // currentPartsList.ForEach(part => part.IsBusy = true);
                _invoker.RedoLastLongCommand();
            }
        }

        private void InvokeRotateCubeLongCommand(Vector3 axis, float angle)
        {
            if (_cubeParts.All(cube => cube.IsBusy == false))
            {
                var currentPartsList = _cubeParts.ToList();
                currentPartsList.ForEach(part => part.IsBusy = true);
                var moveLongCommand = new RotateLongCommand(_mainCube, 270.0f, _mainCube.CoreCenter, axis, angle,
                    onBeforeExecuteOrUndo:() => {
                        currentPartsList.ForEach(part => part.IsBusy = true); 
                        _playRotateSound();
                    },
                    onAfterExecuteOrUndo: () => currentPartsList.ForEach(part => part.IsBusy = false)
                );
                _invoker.AddLongCommand(moveLongCommand);
                _invoker.ExecuteFirstLongCommand();
            }
        }

        private void InvokeSwipeLongCommand(IEnumerable<CubePart> partsToSwipe, Vector3 axis, float angle)
        {
            if (partsToSwipe.All(cube => cube.IsBusy == false))
            {
                var currentPartsList = partsToSwipe.ToList();
                currentPartsList.ForEach(cube => cube.IsBusy = true);
                var rotateLongCommand = new SwipeLongCommand(partsToSwipe, 270.0f, _mainCube.CoreCenter, axis, angle, 
                    onBeforeExecuteOrUndo: () => {
                        currentPartsList.ForEach(cube => cube.IsBusy = true);
                        _playSwipeSound(); 
                    }, 
                    onAfterExecuteOrUndo: () => {
                        currentPartsList.ForEach(part => part.IsBusy = false);
                        OnSwiped?.Invoke();
                    }
                );

                _invoker.AddLongCommand(rotateLongCommand);
                _invoker.ExecuteFirstLongCommand();
            }
        }
    }
}