using UnityEngine;
using UnityEngine.UI;
using TMPro;
using R3;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.MVVM.ViewModels;

namespace Assets._Game.Scripts.MVVM.Views
{
    public class CubeSelectionView : BaseScreen<CubeSelectionViewModel>
    {
        // [SerializeField] private GameObject _uiHolder;
        
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button[] _selectCubeButtons;
        // [SerializeField] private Button[] _selectLevelButtons;
        [SerializeField] private Button _submitButton;
        // [SerializeField] private Renderer _backgroundRenderer;
        // [SerializeField] private Material[] _backgroundMaterials;
        [SerializeField] private TMP_Dropdown _modesDropdown;
        private CompositeDisposable _disposables;

#region BaseScreen implementation
        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public override void Close()
        {
            gameObject.SetActive(false);
        }

        protected override void OnBind(CubeSelectionViewModel viewModel)
        {
            BindButtonsWithViewModel();
            _disposables = new()
            {
                // viewModel.SelectedCube.Subscribe(ChangeBackground),
                viewModel.LastSelectedCube.Subscribe((e) => SwitchButtonAccess(e, true)),
                viewModel.SelectedCube.Subscribe((e) => SwitchButtonAccess(e, false)),
                viewModel.SelectedMode.Subscribe((e) => _modesDropdown.value = e)
            };
        }
#endregion

        // private void Hide()
        // {
        //     _uiHolder.SetActive(false);
        // }
        
        private void BindButtonsWithViewModel()
        {
            _settingsButton.onClick.AddListener(_viewModel.OpenSettings);
            for (int i = 0; i < _selectCubeButtons.Length; i++)
            {
                var number = i;
                _selectCubeButtons[i].onClick.AddListener(() => _viewModel.OnSelectCubeButtonClicked(number));
                _selectCubeButtons[i].interactable = true;
            }
            _submitButton.onClick.AddListener(_viewModel.OnSubmitButtonClicked);
            _modesDropdown.onValueChanged.AddListener((mode) => _viewModel.OnSelectModeClicked(mode));
        }

        // private void SwitchButtonAccess(int buttonIndex)
        // {
        //     for (var i = 0; i < _selectCubeButtons.Length; i++)
        //     {
        //         if (i == buttonIndex)
        //         {
        //             _selectCubeButtons[i].interactable = false;
        //         }
        //         else
        //         {
        //             _selectCubeButtons[i].interactable = true;
        //         } 
        //     }
        // }

        private void SwitchButtonAccess(int buttonIndex, bool isEnabled)
        {
            _selectCubeButtons[buttonIndex].interactable = isEnabled;
        }

        // private void ChangeBackground(int index)
        // {
        //     _backgroundRenderer.material = _backgroundMaterials[index];
        // }

        public override void Dispose()
        {
            _settingsButton.onClick.RemoveAllListeners();
            for (int i = 0; i < _selectCubeButtons.Length; i++)
            {
                _selectCubeButtons[i].onClick.RemoveAllListeners();
            }
            
            _submitButton.onClick.RemoveAllListeners();
            _modesDropdown.onValueChanged.RemoveAllListeners();

            _disposables.Dispose();
        }
    }
}