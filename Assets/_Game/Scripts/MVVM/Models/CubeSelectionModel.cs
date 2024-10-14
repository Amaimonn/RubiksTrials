using R3;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.MVVM.ViewModels;

namespace Assets._Game.Scripts.MVVM.Models
{
    public class CubeSelectionModel : IModel
    {
        public Observable<int> SelectedCube => _selectedCube;
        public Observable<int> SelectedMode => _selectedMode;
        public Observable<int> LastSelectedCube => _lastSelectedCube;

        private readonly ReactiveProperty<int> _selectedCube = new(0);
        private readonly ReactiveProperty<int> _selectedMode = new(0);
        private readonly ReactiveProperty<int> _lastSelectedCube = new(0);
        private Subject<EnterGameplayParameters> _exitSceneSignalSubject;
        
        private readonly CommonRubiksCube[] _cubes;
        private readonly CubePresetSO[] _cubePresets;
        private SettingsModel _settingsModel;
        private UIManager _uiManager;

        public CubeSelectionModel(Subject<EnterGameplayParameters> exitSceneSignalSubject, CommonRubiksCube[] cubes, CubePresetSO[] cubePresets, SettingsModel settingsModel, 
            int selectedCube = 0, int selectionMode = 0)
        {
            _exitSceneSignalSubject = exitSceneSignalSubject;
            _cubes = cubes;
            _settingsModel = settingsModel;
            _cubePresets = cubePresets;
            _selectedCube.Value = selectedCube;
            _selectedMode.Value = selectionMode;

            foreach (var cube in _cubes)
            {
                cube.gameObject.SetActive(false);
            }

            _cubes[_selectedCube.Value].gameObject.SetActive(true);
            _uiManager = ServiceLocator.Current.Get<UIManager>();
        }

        public void OpenSettings()
        {
            // _uiManager.ShowView<SettingsViewModel>();
            _settingsModel.SetSettingsWindowActive(true);
        }

        public void SelectCube(int cubeIndex)
        {
            _cubes[_selectedCube.Value].gameObject.SetActive(false);
            _cubes[cubeIndex].gameObject.SetActive(true);
            
            if (_lastSelectedCube.Value != _selectedCube.Value)
            {
                _lastSelectedCube.Value = _selectedCube.Value;
            }
            else
            {
                _lastSelectedCube?.OnNext(_selectedCube.Value);
            }
            _selectedCube.Value = cubeIndex;
        }

        public void SelectMode(int mode)
        {
            _selectedMode.Value = mode;
        }
        
        // public void RefreshActiveCube()
        // {
        //     _invoker.Clear();
        //     SetConfigurationToActiveCube(_cubePresets[_selectedCube.Value].Cube);
        // }

        // private void SetConfigurationToActiveCube(CommonRubiksCube cubeConfiguration)
        // {
        //     var selectedCube = _cubes[_selectedCube.Value];
        //     selectedCube.transform.SetPositionAndRotation(cubeConfiguration.transform.position, 
        //         cubeConfiguration.transform.rotation);
        //     var parts = selectedCube.CubeParts;
        //     var presetParts = cubeConfiguration.GetComponentsInChildren<CubePart>();
        //     var index = 0;
        //     foreach (var part in parts)
        //     {
        //         part.transform.SetPositionAndRotation(presetParts[index].transform.position, presetParts[index].transform.rotation);
        //         part.IsBusy = false;
        //         index++;
        //     }
           
        //     selectedCube.UpdatePartsDirection();
        // }

        public void InitializeCube()
        {
            var enterGameplayParameters = new EnterGameplayParameters(_selectedCube.Value, _selectedMode.Value);
            _exitSceneSignalSubject.OnNext(enterGameplayParameters);
        }
    }
}