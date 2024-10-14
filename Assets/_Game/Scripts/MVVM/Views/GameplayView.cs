using TMPro;
using UnityEngine;
using UnityEngine.UI;
using R3;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.MVVM.ViewModels;

namespace Assets._Game.Scripts.MVVM.Views
{
    public class GameplayView : BaseScreen<GameplayViewModel>
    {
        [Header("Top panel")]
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _settingsButton;

        // [Header("Rotate faces")]
        // [SerializeField] private Button _rotateNorthIn;
        // [SerializeField] private Button _rotateNorthOut;
        // [SerializeField] private Button _rotateSouthIn;
        // [SerializeField] private Button _rotateSouthOut;
        // [SerializeField] private Button _rotateEastIn;
        // [SerializeField] private Button _rotateEastOut;
        // [SerializeField] private Button _rotateWestIn;
        // [SerializeField] private Button _rotateWestOut;
        // [SerializeField] private Button _rotateTopIn;
        // [SerializeField] private Button _rotateTopOut;
        // [SerializeField] private Button _rotateBottomIn;
        // [SerializeField] private Button _rotateBottomOut;

        [Header("Move cube")]
        [SerializeField] private Button _moveXPositive;
        [SerializeField] private Button _moveXNegative;
        [SerializeField] private Button _moveYPositive;
        [SerializeField] private Button _moveYNegative;
        [SerializeField] private Button _moveZPositive;
        [SerializeField] private Button _moveZNegative;

        [Header("Undo/Redo")]
        [SerializeField] private Button _undoButton;
        [SerializeField] private TMP_Text _undoCountText;

        [Space(4f)]
        [SerializeField] private Button _redoButton;
        [SerializeField] private TMP_Text _redoCountText;

        [Header("Decision pop up")]
        [SerializeField] private DecisionPopUp _decisionPopUp;

        private CompositeDisposable _disposables;

        protected override void OnBind(GameplayViewModel viewModel)
        {
            AddButtonListeners();

            _disposables = new()
            {
                // viewModel.ExitGameplay.Subscribe((e) => {
                //     // _decisionPopUp.Close();
                //     // _gameOverPopUpText.Show(_timeText.text);
                // }),
                viewModel.UndoCount.Subscribe(SetUndoCountText),
                viewModel.IsUndoAvailable.Subscribe(SwitchUndoAvailability),
                viewModel.RedoCount.Subscribe(SetRedoCountText),
                viewModel.IsRedoAvailable.Subscribe(SwitchRedoAvailability),
                viewModel.IsExitWindowActive.Subscribe(SetExitWindowActive),
            };

            _decisionPopUp.Bind(() => viewModel.OnExitButtonClicked(), 
                () => { 
                    // _exitButton.enabled = true;
                    viewModel.SetExitWindowActive(false);
                }
            );
        }

        private void Awake()
        {
            _decisionPopUp.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _exitButton.enabled = true;
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void AddButtonListeners()
        {
            // _rotateNorthIn.onClick.AddListener(_viewModel.OnSwipeNorthPositiveButtonClicked);
            // _rotateNorthOut.onClick.AddListener(_viewModel.OnSwipeNorthNegativeButtonClicked);
            // _rotateSouthIn.onClick.AddListener(_viewModel.OnSwipeSouthPositiveButtonClicked);
            // _rotateSouthOut.onClick.AddListener(_viewModel.OnSwipeSouthNegativeButtonClicked);
            // _rotateEastIn.onClick.AddListener(_viewModel.OnSwipeEastPositiveButtonClicked);
            // _rotateWestOut.onClick.AddListener(_viewModel.OnSwipeWestNegativeButtonClicked);
            // _rotateWestIn.onClick.AddListener(_viewModel.OnSwipeWestPositiveButtonClicked);
            // _rotateEastOut.onClick.AddListener(_viewModel.OnSwipeEastNegativeButtonClicked);
            // _rotateTopIn.onClick.AddListener(_viewModel.OnSwipeTopPositiveButtonClicked);
            // _rotateTopOut.onClick.AddListener(_viewModel.OnSwipeTopNegativeButtonClicked);
            // _rotateBottomIn.onClick.AddListener(_viewModel.OnSwipeBottomNegativeButtonClicked);
            // _rotateBottomOut.onClick.AddListener(_viewModel.OnSwipeBottomPositiveButtonClicked);
            
            _moveXPositive.onClick.AddListener(_viewModel.OnRotateXPositiveButtonClicked);
            _moveXNegative.onClick.AddListener(_viewModel.OnRotateXNegativeButtonClicked);
            _moveYPositive.onClick.AddListener(_viewModel.OnRotateYPositiveButtonClicked);
            _moveYNegative.onClick.AddListener(_viewModel.OnRotateYNegativeButtonClicked);
            _moveZPositive.onClick.AddListener(_viewModel.OnRotateZPositiveButtonClicked);
            _moveZNegative.onClick.AddListener(_viewModel.OnRotateZNegativeButtonClicked);

            _exitButton.onClick.AddListener(() => _viewModel.SetExitWindowActive(true));
            _settingsButton.onClick.AddListener(_viewModel.OpenSettings);
            _undoButton.onClick.AddListener(_viewModel.OnUndoButtonClicked);
            _redoButton.onClick.AddListener(_viewModel.OnRedoButtonClicked);
        }

        private void SwitchUndoAvailability(bool isEnabled)
        {
            _undoButton.interactable = isEnabled;
        }

        private void SwitchRedoAvailability(bool isEnabled)
        {
            _redoButton.interactable = isEnabled;
        }

        private void SetUndoCountText(int count)
        {
            _undoCountText.text = count.ToString();
        }

        private void SetRedoCountText(int count)
        {
            _redoCountText.text = count.ToString();
        }

        private void SetExitWindowActive(bool isActive)
        {
            //_exitButton.enabled = false;
            if (isActive)
                _decisionPopUp.Show();
            else
                _decisionPopUp.Close();
        }

        public override void Dispose()
        {
            // _rotateNorthIn.onClick.RemoveAllListeners();
            // _rotateNorthOut.onClick.RemoveAllListeners();
            // _rotateSouthIn.onClick.RemoveAllListeners();
            // _rotateSouthOut.onClick.RemoveAllListeners();
            // _rotateEastIn.onClick.RemoveAllListeners();
            // _rotateEastOut.onClick.RemoveAllListeners();
            // _rotateWestIn.onClick.RemoveAllListeners();
            // _rotateWestOut.onClick.RemoveAllListeners();
            // _rotateTopIn.onClick.RemoveAllListeners();
            // _rotateTopOut.onClick.RemoveAllListeners();
            // _rotateBottomIn.onClick.RemoveAllListeners();
            // _rotateBottomOut.onClick.RemoveAllListeners();
            
            _moveXPositive.onClick.RemoveAllListeners();
            _moveXNegative.onClick.RemoveAllListeners();
            _moveYPositive.onClick.RemoveAllListeners();
            _moveYNegative.onClick.RemoveAllListeners();
            _moveZPositive.onClick.RemoveAllListeners();
            _moveZNegative.onClick.RemoveAllListeners();

            _exitButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.RemoveAllListeners();
            _undoButton.onClick.RemoveAllListeners();
            _redoButton.onClick.RemoveAllListeners();

            _disposables.Dispose();
            _decisionPopUp.Dispose();
        }
    }
}