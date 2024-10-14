using System;
using System.Collections;
using UnityEngine;
using R3;
using Assets._Game.Scripts.Configs;
using Assets._Game.Scripts.MVVM.ViewModels;
using Assets._Game.Scripts.SolveStates;

namespace Assets._Game.Scripts.MVVM.Models
{
    public class TutorialLevel : AbstractLevelModel
    {
        public int StagesCount => _stagesCount;
        public Observable<int> StageIndex => _stageIndex;
        public Observable<bool> IsNextStageEnabled => _isNextStageEnabled;
        public Observable<bool> IsExitEnabled => _isExitEnabled;

        protected ReactiveProperty<int> _stageIndex = new (0);
        protected ReactiveProperty<bool> _isNextStageEnabled = new (false);
        protected ReactiveProperty<bool> _isExitEnabled = new (false);
        protected CubePresetSO[] _cubePresets;
        protected Action<CommonRubiksCube> _setConfiguration;
        protected Action _openHelpMenu;
        protected readonly TabbedMenuModel _helpMenuModel;
        protected readonly TabbedMenuViewModel _helpMenuViewModel;
        protected readonly SolveState[] _solveStates;
        protected readonly int _stagesCount;

        public TutorialLevel(CommonRubiksCube rubiksCube, Action onLevelExit, CubePresetSO[] cubePresets, 
            Action<CommonRubiksCube> setConfiguration, ITabbedMenuData tabbedMenuData, SolveState[] solveStates) : 
            base(rubiksCube, onLevelExit)
        {
            _cubePresets = cubePresets;
            _setConfiguration = setConfiguration;
            _solveStates = solveStates;
            _stagesCount = solveStates.Length;

            _helpMenuModel = new TabbedMenuModel(tabbedMenuData.TabsData);
            _helpMenuViewModel = new TabbedMenuViewModel(_helpMenuModel);
            _helpMenuModel.IsActive.Subscribe(e => SetInputEnabled(!e));
            ServiceLocator.Current.Get<UIManager>().BindHidden(_helpMenuViewModel);
            _openHelpMenu = () => _helpMenuModel.OpenMenu();
        }

        public override IEnumerator StartLevel()
        {
            //TODO: animation
            yield return new WaitForSeconds(1.5f);
            _setConfiguration(_cubePresets[0].Cube);
            yield return null;
        }

        public override void TryComplete()
        {
            if (_solveStates[_stageIndex.Value].Compare(_rubiksCube))
            {
                if (_stageIndex.Value < _stagesCount - 1)
                {
                    SetActiveNextStage(true);
                }
            }
            else
            {
                SetActiveNextStage(false);
            }

            if (_rubiksCube.CheckIsSolved())
            {
                SetActiveExit(true);
            }
            else
            {
                SetActiveExit(false);
            }
        }

        public override void Finish() // for exit button
        {
            bool isSuccess = _rubiksCube.CheckIsSolved();
            ServiceLocator.Current.Get<UIManager>().Hide<TabbedMenuViewModel>();
            _helpMenuViewModel.Dispose();
            // OnComplete?.OnNext(isSuccess);
            OnLevelExit?.OnNext(isSuccess);
        }

        public void SetStage(int stage) // for buttons
        {
            SetActiveNextStage(false);
            SetActiveExit(false);
            _stageIndex.Value = stage;
            _setConfiguration(_cubePresets[stage].Cube);
        }

        public void RestartStage()
        {
            SetStage(_stageIndex.Value);
        }

        public void OpenHelpMenu()
        {
            _openHelpMenu();
        }

        private void SetActiveNextStage(bool isActive)
        {
            _isNextStageEnabled.Value = isActive;
        }

        private void SetActiveExit(bool isActive)
        {
            _isExitEnabled.Value = isActive;
        }

        private void SetInputEnabled(bool isEnabled)
        {
            _isInputEnabled.Value = isEnabled;
        }
    }
}