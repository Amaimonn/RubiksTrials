using System;
using UnityEngine;

namespace Assets._Game.Scripts.MVVM.Abstractions
{
    public abstract class AbstractScreen : MonoBehaviour, IDisposable
    {
        public abstract Type ViewModelType { get; }

        public abstract void Show();
        public abstract void Close();
        public abstract void Bind(object model);

        public virtual void Dispose()
        {
        }
    }
    
}