using UnityEngine;
using UnityEngine.UI;
using R3;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.MVVM.ViewModels;

namespace Assets._Game.Scripts.MVVM.Views
{
    public class ButtonsControlView : BaseScreen<ButtonsControlViewModel>
    {
        [Header("Rotate faces")]
        [SerializeField] private Button _rotateNorthIn;
        [SerializeField] private Button _rotateNorthOut;
        [SerializeField] private Button _rotateSouthIn;
        [SerializeField] private Button _rotateSouthOut;
        [SerializeField] private Button _rotateEastIn;
        [SerializeField] private Button _rotateEastOut;
        [SerializeField] private Button _rotateWestIn;
        [SerializeField] private Button _rotateWestOut;
        [SerializeField] private Button _rotateTopIn;
        [SerializeField] private Button _rotateTopOut;
        [SerializeField] private Button _rotateBottomIn;
        [SerializeField] private Button _rotateBottomOut;

        private bool _isControlEnabled;
        private CompositeDisposable _disposables;

        protected override void OnBind(ButtonsControlViewModel viewModel)
        {
            AttachButtonListeners();
            _disposables = new()
            {
                viewModel.IsControlEnabled.Subscribe(e => _isControlEnabled = e)
            };
        }

        private void AttachButtonListeners()
        {
            _rotateNorthIn.onClick.AddListener(_viewModel.OnSwipeNorthPositiveButtonClicked);
            _rotateNorthOut.onClick.AddListener(_viewModel.OnSwipeNorthNegativeButtonClicked);
            _rotateSouthIn.onClick.AddListener(_viewModel.OnSwipeSouthPositiveButtonClicked);
            _rotateSouthOut.onClick.AddListener(_viewModel.OnSwipeSouthNegativeButtonClicked);
            _rotateEastIn.onClick.AddListener(_viewModel.OnSwipeEastPositiveButtonClicked);
            _rotateWestOut.onClick.AddListener(_viewModel.OnSwipeWestNegativeButtonClicked);
            _rotateWestIn.onClick.AddListener(_viewModel.OnSwipeWestPositiveButtonClicked);
            _rotateEastOut.onClick.AddListener(_viewModel.OnSwipeEastNegativeButtonClicked);
            _rotateTopIn.onClick.AddListener(_viewModel.OnSwipeTopPositiveButtonClicked);
            _rotateTopOut.onClick.AddListener(_viewModel.OnSwipeTopNegativeButtonClicked);
            _rotateBottomIn.onClick.AddListener(_viewModel.OnSwipeBottomNegativeButtonClicked);
            _rotateBottomOut.onClick.AddListener(_viewModel.OnSwipeBottomPositiveButtonClicked);
        }

        private void DetachButtonListeners()
        {
            _rotateNorthIn.onClick.RemoveAllListeners();
            _rotateNorthOut.onClick.RemoveAllListeners();
            _rotateSouthIn.onClick.RemoveAllListeners();
            _rotateSouthOut.onClick.RemoveAllListeners();
            _rotateEastIn.onClick.RemoveAllListeners();
            _rotateEastOut.onClick.RemoveAllListeners();
            _rotateWestIn.onClick.RemoveAllListeners();
            _rotateWestOut.onClick.RemoveAllListeners();
            _rotateTopIn.onClick.RemoveAllListeners();
            _rotateTopOut.onClick.RemoveAllListeners();
            _rotateBottomIn.onClick.RemoveAllListeners();
            _rotateBottomOut.onClick.RemoveAllListeners();
        }

        public override void Dispose()
        {
            DetachButtonListeners();
            _disposables?.Dispose();
        }
    }
}