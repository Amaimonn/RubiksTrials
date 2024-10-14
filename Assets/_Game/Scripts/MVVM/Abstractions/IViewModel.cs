using System;

namespace Assets._Game.Scripts.MVVM.Abstractions
{
    public interface IViewModel : IDisposable
    {   
        public Type ViewModelType {get;}
    }
}