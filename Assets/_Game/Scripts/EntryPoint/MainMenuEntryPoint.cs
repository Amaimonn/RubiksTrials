using System.Collections.Generic;
using UnityEngine;
using R3;
using Assets._Game.Scripts.MVVM;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.MVVM.Models;
using Assets._Game.Scripts.MVVM.ViewModels;

namespace Assets._Game.Scripts.EntryPoint
{
    public class MainMenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private GameObject _sceneUIRootPrefab;
        [SerializeField] private CommonRubiksCube[] _rubiksCubes;
        [SerializeField] private CubePresetSO[] _initialPresets;
        private IEnumerable<AbstractScreen> _screens;

        public Observable<EnterGameplayParameters> Boot(EnterMainMenuParameters enterMainMenuParameters)
        {
            var uiScene = Instantiate(_sceneUIRootPrefab);
            _screens = uiScene.GetComponentsInChildren<AbstractScreen>();

            var serviceLocator = ServiceLocator.Current;
            var uiManager = serviceLocator.Get<UIManager>();
            
            uiManager.AttachSceneUI(uiScene);
            uiManager.Initialize(_screens);

            var sceneExitParams = new Subject<EnterGameplayParameters>();

            var settingsModel = serviceLocator.Get<IGameStateProvider>().Settings;
            var settingsViewModel = new SettingsViewModel(settingsModel);
            uiManager.BindHidden(settingsViewModel);
            sceneExitParams.Subscribe(_ => settingsViewModel.Dispose());
            
            enterMainMenuParameters ??= new EnterMainMenuParameters(0, 0);
            var selectionModel = new CubeSelectionModel(sceneExitParams, _rubiksCubes, _initialPresets, settingsModel, 
                enterMainMenuParameters.CubeIndex, enterMainMenuParameters.LevelIndex);
            var selectionViewModel = new CubeSelectionViewModel(selectionModel);

            uiManager.BindAndShow(selectionViewModel);


            return sceneExitParams;
        }
    }
}