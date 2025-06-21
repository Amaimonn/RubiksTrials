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
                viewModel.UndoCount.Subscribe(SetUndoCountText),
                viewModel.IsUndoAvailable.Subscribe(SwitchUndoAvailability),
                viewModel.RedoCount.Subscribe(SetRedoCountText),
                viewModel.IsRedoAvailable.Subscribe(SwitchRedoAvailability),
                viewModel.IsExitWindowActive.Subscribe(SetExitWindowActive),
            };

            _decisionPopUp.Bind(() => viewModel.OnExitButtonClicked(), 
                () => viewModel.SetExitWindowActive(false)
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
            if (isActive)
                _decisionPopUp.Show();
            else
                _decisionPopUp.Close();
        }

        public override void Dispose()
        {
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

            _disposables?.Dispose();
            _decisionPopUp.Dispose();
        }
    }
}