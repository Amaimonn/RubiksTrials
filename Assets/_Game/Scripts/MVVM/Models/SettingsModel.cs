using System;
using UnityEngine;
using R3;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.MVVM.ViewModels;

namespace Assets._Game.Scripts.MVVM.Models
{
    public class SettingsModel : IModel
    {
        public Observable<bool> IsOpened => _isOpened;
        public Observable<float> SoundVolume => _soundVolume;
        public Observable<float> SFXVolume => _sfxVolume;
        public Observable<bool> IsSoundEnabled => _isSoundEnabled;
        public Observable<bool> IsSFXEnabled => _isSFXEnabled;
        public Observable<Func<GameplayModel, ViewModel<GameplayModel>>> CurrentControlFactory => _currentControlFactory;
        public Observable<int> CurrentControlIndex => _currentControlIndex;

        private ReactiveProperty<bool> _isOpened = new(false);
        private ReactiveProperty<float> _soundVolume = new(0.5f);
        private ReactiveProperty<float> _sfxVolume = new(0.5f);
        private ReactiveProperty<bool> _isSoundEnabled = new(true);
        private ReactiveProperty<bool> _isSFXEnabled = new(true);
        private ReactiveProperty<Func<GameplayModel, ViewModel<GameplayModel>>> _currentControlFactory = new((e) => new DragControlViewModel(e));
        private ReactiveProperty<int> _currentControlIndex = new();

        private SoundPlayer _soundPlayer;

        public SettingsModel()
        {
            _soundPlayer = ServiceLocator.Current.Get<SoundPlayer>();
        }

        public void SetSettingsWindowActive(bool isActive)
        {
            _isOpened.Value = isActive;
        }
        
        public void SetSoundVolume(float volume)
        {
            _soundVolume.Value = volume;
            _soundPlayer.SetSoundVolume(volume);
        }

        public void SetSFXVolume(float volume)
        {
            _sfxVolume.Value = volume;
            _soundPlayer.SetSFXVolume(volume);
        }

        public void SetSoundEnabled(bool isEnabled)
        {
            _isSoundEnabled.Value = isEnabled;

            if (!isEnabled)
            {
                _soundPlayer.SetSoundVolume(0);
            }
            else
            {
                if (_soundVolume.Value > 0)
                {
                    _soundPlayer.SetSoundVolume(_soundVolume.Value);
                }
            }
        }

        public void SetSFXEnabled(bool isEnabled)
        {
            _isSFXEnabled.Value = isEnabled;
            
            if (!isEnabled)
            {
                _soundPlayer.SetSFXVolume(0);
            }
            else
            {
                if (_sfxVolume.Value > 0)
                {
                    _soundPlayer.SetSFXVolume(_sfxVolume.Value);
                }
            }
        }

        public void SetDragControl()
        {
            if (_currentControlIndex.Value == ControlsIndex.Drag)
                return;

            _currentControlFactory.Value = (e) => new  DragControlViewModel(e);
            _currentControlIndex.Value = ControlsIndex.Drag;
        }
        
        public void SetButtonsControl()
        {
            if (_currentControlIndex.Value == ControlsIndex.Buttons)
                return;
                
            _currentControlFactory.Value = (e) => new ButtonsControlViewModel(e);
            _currentControlIndex.Value = ControlsIndex.Buttons;
        }
    }
}
