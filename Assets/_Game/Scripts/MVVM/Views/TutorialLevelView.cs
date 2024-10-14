using UnityEngine;
using UnityEngine.UI;
using TMPro;
using R3;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.MVVM.ViewModels;
using System.Collections.Generic;

namespace Assets._Game.Scripts.MVVM.Views
{
    public class TutorialLevelView : BaseScreen<TutorialLevelViewModel>
    {
        [SerializeField] private Button _nextStageButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _helpButton;
        [SerializeField] private string _helpText;
        [SerializeField] private Image _tipImage;
        [SerializeField] private Image _currentStageImage;
        [SerializeField] private TMP_Dropdown _stagesDropdown;

        private CompositeDisposable _disposables;
        
        protected override void OnBind(TutorialLevelViewModel viewModel)
        {
            
            _disposables = new()
            {
                viewModel.IsNextStageEnabled.Subscribe(SetNextButtonActive),
                viewModel.IsExitEnabled.Subscribe(SetExitButtonActive),
                viewModel.StageIndex.Subscribe(SetCurrentStage),
                
                // _stagesDropdown.OnSelectAsObservable().Subscribe((e) => {

                
                // viewModel.OnSetStageButtonClicked(_stagesDropdown.value);
                // }),
                viewModel.StageIndex.Subscribe((e) => _stagesDropdown.SetValueWithoutNotify(e))
            };
            _nextStageButton.onClick.AddListener(viewModel.OnNextStageButtonClicked);
            _exitButton.onClick.AddListener(viewModel.OnExitButtonClicked);
            _helpButton.onClick.AddListener(viewModel.OnHelpButtonClicked);
            _stagesDropdown.onValueChanged.AddListener(viewModel.OnSetStageButtonClicked);
            
            List<string> stages = new();
            for (int i = 1; i <= viewModel.StagesCount; i++)
            {
                stages.Add($"{i} этап");
            }
            _stagesDropdown.AddOptions(stages);
            // _totalStagesCount =
        }

        private void SetNextButtonActive(bool isActive) => _nextStageButton.gameObject.SetActive(isActive);

        private void SetExitButtonActive(bool isActive) => _exitButton.gameObject.SetActive(isActive);

        private void SetTipImage(Image stageImage) => _tipImage = stageImage;

        private void SetHelpText(string helpText) => _helpText = helpText;

        private void SetCurrentStage(int stageIndex)
        {
            // _currentStageImage.sprite = Resources.Load<Sprite>($"Tutorial/Stage{stageIndex}");
        }

        public override void Dispose()
        {
            _disposables.Dispose();
            _nextStageButton.onClick.RemoveAllListeners();
            _exitButton.onClick.RemoveAllListeners();
            _helpButton.onClick.RemoveAllListeners();
            _stagesDropdown.onValueChanged.RemoveAllListeners();
        }
    }
}