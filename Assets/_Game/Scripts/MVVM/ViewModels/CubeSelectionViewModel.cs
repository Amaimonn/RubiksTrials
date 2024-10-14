using R3;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.MVVM.Models;
using System;
using UnityEngine;

namespace Assets._Game.Scripts.MVVM.ViewModels
{
    public class CubeSelectionViewModel : ViewModel<CubeSelectionModel>
    {
        public override Type ViewModelType => typeof(CubeSelectionViewModel);
        public Observable<int> SelectedCube => _selectedCube;
        public Observable<int> SelectedMode => _selectedMode;
        public Observable<int> LastSelectedCube => _lastSelectedCube;

        private ReactiveProperty<int> _lastSelectedCube = new();
        private ReactiveProperty<int> _selectedCube = new();
        private ReactiveProperty<int> _selectedMode = new();
        private CompositeDisposable _disposables = new();

        public CubeSelectionViewModel(CubeSelectionModel model) : base(model)
        {
        }

        protected override void OnBind(CubeSelectionModel model)
        {
            _disposables.Add(model.LastSelectedCube.Subscribe(ChangeLast));
            _disposables.Add(model.SelectedCube.Subscribe(OnCubeSelected));
            _disposables.Add(model.SelectedMode.Subscribe((e) => _selectedMode.Value = e));
        }

        public void OpenSettings()
        {
            _model.OpenSettings();
        }
        
        public void OnSelectModeClicked(int mode)
        {
            if (mode == 0)
            {
                if (_selectedCube.Value != 0)
                {
                    _model.SelectCube(0);
                }
            }
            _model.SelectMode(mode);
        }

        public void OnSelectCubeButtonClicked(int cubeIndex)
        {
            if (_selectedMode.Value != 0)
                _model.SelectCube(cubeIndex);
            else
            {
                Debug.Log("Tutorial only with 3x3 cube");
            }
        }

        public void OnSubmitButtonClicked()
        {
            Dispose();
            _model.InitializeCube();
        }

        private void ChangeLast(int last)
        {
            if (last == _lastSelectedCube.Value)
            {
                _lastSelectedCube?.OnNext(last);
            }
            else
            {
                _lastSelectedCube.Value = last;
            }
        }

        private void OnCubeSelected(int cubeIndex)
        {
            _selectedCube.Value = cubeIndex;
        }
        
        public override void Dispose()
        {
            _disposables.Dispose();
        }
    }
}