using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Game.Scripts.Processors
{
    public interface IControlsProcessor
    {
        public void Swipe(IEnumerable<CubePart> partsToSwipe, Vector3 axis, float angle);
        public void Rotate(Vector3 axis, float angle);
        public void Undo();
        public void Redo();
        public event Action OnSwiped;
    }
}