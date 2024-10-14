using System;

namespace Assets._Game.Scripts.MVVM.Abstractions
{
    public abstract class BaseScreen<TViewModel> : AbstractScreen where TViewModel : IViewModel
    {
        public override Type ViewModelType { get => typeof(TViewModel); }
        protected TViewModel _viewModel;

        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public override void Close()
        {
            gameObject.SetActive(false);
        }

        public override void Bind(object model)
        {
            if (model is TViewModel viewModel)
                Bind(viewModel);
        }

        public void Bind(TViewModel viewModel)
        {
            _viewModel = viewModel;
            OnBind(_viewModel);
        }

        protected abstract void OnBind(TViewModel viewModel);
    }
}
