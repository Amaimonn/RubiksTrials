using System;

namespace Assets._Game.Scripts.MVVM.Abstractions
{
    public abstract class ViewModel<TModel> : IViewModel where TModel : IModel
    {
        public abstract Type ViewModelType { get; }
        protected TModel _model;

        public ViewModel(TModel model)
        {
            Bind(model);
        }

        public void Bind(TModel model)
        {
            _model = model;
            OnBind(model);
        }

        protected abstract void OnBind(TModel model);

        public virtual void Dispose()
        {
        }
    }
}