using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using R3;
using Assets._Game.Scripts.MVVM;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.MVVM.ViewModels;
using Assets._Game.Scripts.MVVM.Models;
using Assets._Game.Scripts.Processors;
using Assets._Game.Scripts.SolveStates;

public class GameplayEntryPoint : MonoBehaviour
{
    [SerializeField] private GameObject[] _sceneUIRoots;
    [SerializeField] private CommonRubiksCube[] _rubiksCubes;
    [SerializeField] private Invoker _invoker;
    [SerializeField] private CubePresetSO[] _initialPresets;
    [SerializeField] private CubePresetSO[] _tutorialCubePresets;
    [SerializeField] private TabbedMenuSO[] _tabbedMenuPresets;

    private UIManager _uiManager;
    private CommonRubiksCube _selectedCube;
    private int _selectedCubeIndex;
    private GameplayModel _gameplayModel;
    private SettingsModel _settingsModel;
    private AbstractLevelModel _levelModel;
    private ViewModel<GameplayModel> _currentControlsViewModel;
    private CompositeDisposable _disposables = new();

    public Observable<EnterMainMenuParameters> Boot(EnterGameplayParameters enterGameplayParameters)
    {
        var uiScene = Instantiate(_sceneUIRoots[enterGameplayParameters.LevelIndex]);
        var levelViews = uiScene.GetComponentsInChildren<AbstractScreen>().ToList();
        var serviceLocator = ServiceLocator.Current;
        _uiManager = serviceLocator.Get<UIManager>();
        _uiManager.AttachSceneUI(uiScene);
        _uiManager.Initialize(levelViews);

        foreach (var cube in _rubiksCubes)
        {
            cube.gameObject.SetActive(false);
        }
        
        _selectedCubeIndex = enterGameplayParameters.CubeIndex;
        _selectedCube = _rubiksCubes[_selectedCubeIndex];
        _selectedCube.gameObject.SetActive(true);

        _settingsModel = serviceLocator.Get<IGameStateProvider>().Settings;
        var settingsViewModel = new SettingsViewModel(_settingsModel);
        _uiManager.BindHidden(settingsViewModel);

        var controller = new AutoControlsProcessor(_invoker, _selectedCube);
        var exitSceneSignalSubject = new Subject<Unit>();
        var levelModel = CreateLevel(enterGameplayParameters.LevelIndex);
        var levelViewModel = CreateLevelViewModel(levelModel);
        _gameplayModel = new GameplayModel(_selectedCube, controller, _invoker, levelModel, _settingsModel, 
            () => exitSceneSignalSubject.OnNext(Unit.Default));


        var gameplayViewModel = new GameplayViewModel(_gameplayModel);

        StartCoroutine(StartGameplay(gameplayViewModel, levelViewModel));

        var enterMainMenuParameters = new EnterMainMenuParameters(enterGameplayParameters.CubeIndex, enterGameplayParameters.LevelIndex);
        var returnToMainMenuSignal = exitSceneSignalSubject.Select(_ => enterMainMenuParameters);
        return returnToMainMenuSignal;
    }

    private IEnumerator StartGameplay<T> (params T[] viewModels) where T : IViewModel
    {
        Debug.Log("Start gameplay routine");
        yield return new WaitForSeconds(_uiManager.TransitionDelay + 0.5f);
        yield return _levelModel.StartLevel();

        foreach (var viewModel in viewModels)
        {
            _uiManager.BindAndShow(viewModel);
        }

        _settingsModel.CurrentControlFactory.Subscribe(factory => BindControlsFromFactory(factory)).AddTo(_disposables);
        yield return _uiManager.ShowGroupAnimation();
    }

    private void BindControlsFromFactory(Func<GameplayModel, ViewModel<GameplayModel>> controlsfactory)
    {
        if (_currentControlsViewModel != null)
        {
            _uiManager.Hide(_currentControlsViewModel);
            _currentControlsViewModel.Dispose();
        }
        _currentControlsViewModel = controlsfactory(_gameplayModel);
        _uiManager.BindAndShow(_currentControlsViewModel);
    }

    private AbstractLevelModel CreateLevel(int typeIndex)
        {
            _levelModel = typeIndex switch
            {
                0 => new TutorialLevel(_selectedCube,
                    () => {
                        _selectedCube.StopAllCoroutines(); 
                    },
                    _tutorialCubePresets, 
                    (config) => {
                        _invoker.Clear();
                        SetConfigurationToActiveCube(config);
                    }, 
                    _tabbedMenuPresets[_selectedCubeIndex],
                    
                    new SolveState[] {
                        new CrossState(),
                        new EdgeWithMiddlePartsState(),
                        new AllEdgesExceptOneState(),
                        new LastEdgeWithPlacedCross(),
                        new LastEdgeWithAccurateCross(),
                        new LastEdgeWithPlacedAngles(),
                        new FullSolvedState()
                    }),

                1 => new ClassicLevel(_selectedCube, 
                    () => {_selectedCube.StopAllCoroutines(); },
                    () => RefreshActiveCube()),

                2 => new LimitedSwipesLevel(_selectedCube,
                    () => {_selectedCube.StopAllCoroutines(); },
                    () => RefreshActiveCube()),

                3 => new CollectColorsLevel(_selectedCube,
                    () => {_selectedCube.StopAllCoroutines(); },
                    _invoker.Clear),

                4 => new FreeLevel(_selectedCube,
                    () => {_selectedCube.StopAllCoroutines(); },
                    () => RefreshActiveCube()),

                _ => new FreeLevel(_selectedCube,
                    () => {_selectedCube.StopAllCoroutines(); },
                    () => RefreshActiveCube()),
            };

            _levelModel.OnComplete.Subscribe(_selectedCube.PlayOnSolvedEffects); // optional

            return _levelModel;
        }

        private IViewModel CreateLevelViewModel(AbstractLevelModel levelModel)
        {
            IViewModel levelViewModel = levelModel switch 
            {
                TimeProgressLevel timeProgressLevel => new TimeProgressLevelViewModel(timeProgressLevel),
                TutorialLevel tutorialLevel => new TutorialLevelViewModel(tutorialLevel),
                FreeLevel freeLevel => new FreeLevelViewModel(freeLevel),
                _ => new FreeLevelViewModel(levelModel as FreeLevel),
            };
            return levelViewModel;
        }

        private void RefreshActiveCube()
        {
            _invoker.Clear();
            SetConfigurationToActiveCube(_initialPresets[_selectedCubeIndex].Cube);
        }

        private void SetConfigurationToActiveCube(CommonRubiksCube cubeConfiguration)
        {
            _selectedCube.transform.SetPositionAndRotation(cubeConfiguration.transform.position, 
                cubeConfiguration.transform.rotation);
            var parts = _selectedCube.CubeParts;
            var presetParts = cubeConfiguration.GetComponentsInChildren<CubePart>();
            var index = 0;
            foreach (var part in parts)
            {
                part.transform.SetPositionAndRotation(presetParts[index].transform.position, presetParts[index].transform.rotation);
                part.IsBusy = false;
                index++;
            }
           
            _selectedCube.UpdatePartsDirection();
        }

#region MonoBehaviour
        private void OnDestroy()
        {
            Dispose();
        }
#endregion
        private void Dispose()
        {
            _disposables?.Dispose();
            _currentControlsViewModel?.Dispose();
        }
}
