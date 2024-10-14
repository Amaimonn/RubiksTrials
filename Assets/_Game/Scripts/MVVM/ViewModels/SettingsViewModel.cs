using System;
using R3;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.MVVM.Models;

namespace Assets._Game.Scripts.MVVM.ViewModels
{
    public class SettingsViewModel : ViewModel<SettingsModel>
    {

        public override Type ViewModelType => typeof(SettingsViewModel);

        public Observable<bool> IsOpened => _isOpened;
        public Observable<float> SoundVolume => _soundVolume;
        public Observable<float> SFXVolume => _sfxVolume;
        public Observable<bool> IsSoundEnabled => _isSoundEnabled;
        public Observable<bool> IsSFXEnabled => _isSFXEnabled;
        public Observable<int> CurrentControlIndex => _currentControlIndex;

        private ReactiveProperty<bool> _isOpened = new(false);
        private ReactiveProperty<float> _soundVolume = new();
        private ReactiveProperty<float> _sfxVolume = new();
        private ReactiveProperty<bool> _isSoundEnabled = new();
        private ReactiveProperty<bool> _isSFXEnabled = new();
        private ReactiveProperty<int> _currentControlIndex = new();

        private CompositeDisposable _disposables;

        public SettingsViewModel(SettingsModel model) : base(model)
        {
        }

        protected override void OnBind(SettingsModel model)
        {
            model.SoundVolume.Subscribe(e => _soundVolume.Value = e).Dispose();
            model.SFXVolume.Subscribe(e => _sfxVolume.Value = e).Dispose();
            
            _disposables = new ()
            {
                model.IsOpened.Subscribe(e => _isOpened.Value = e),
                model.SoundVolume.Skip(1).Subscribe(e => HandleSoundVolumeChanges(e)),
                model.SFXVolume.Skip(1).Subscribe(e => HandleSFXVolumeChanges(e)),
                model.IsSoundEnabled.Subscribe(e => _isSoundEnabled.Value = e),
                model.IsSFXEnabled.Subscribe(e => _isSFXEnabled.Value = e),
                model.CurrentControlIndex.Subscribe(e => _currentControlIndex.Value = e)
            };
        }
        
        public void SetSettingsWindowActive(bool isActive)
        {
            _model.SetSettingsWindowActive(isActive);
        }

        public void SetSoundVolume(float volume)
        {
            _model.SetSoundVolume(volume);
        }

        public void SetSFXVolume(float volume)
        {
            _model.SetSFXVolume(volume);
        }

        public void SwitchSoundEnabled()
        {
            _model.SetSoundEnabled(!_isSoundEnabled.Value);
        }

        public void SwitchSFXEnabled()
        {
            _model.SetSFXEnabled(!_isSFXEnabled.Value);
        }

        public void SetDragControl()
        {
            _model.SetDragControl();
        }
        
        public void SetButtonsControl()
        {
            _model.SetButtonsControl();
        }

        private void HandleSoundVolumeChanges(float volume)
        {
            _soundVolume.Value = volume;
            if (volume > 0 && _isSoundEnabled.Value == false)
            {
                _model.SetSoundEnabled(true);
            }
            else if (volume <= 0)
            {
                _model.SetSoundEnabled(false);
            }
        }

        private void HandleSFXVolumeChanges(float volume)
        {
            _sfxVolume.Value = volume;
            if (volume > 0 && _isSFXEnabled.Value == false)
            {
                _model.SetSFXEnabled(true);
            }
            else if (volume <= 0)
            {
                _model.SetSFXEnabled(false);
            }
        }

        public override void Dispose()
        {
            _disposables.Dispose();
        }

    }
}