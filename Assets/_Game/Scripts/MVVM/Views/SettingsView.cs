using UnityEngine;
using R3;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.MVVM.ViewModels;
using UnityEngine.UI;

namespace Assets._Game.Scripts.MVVM.Views
{
    public class SettingsView : BaseScreen<SettingsViewModel>
    {

        [SerializeField] private Button _closeButton;

        [Header("Sound")]
        [SerializeField] private Slider _soundSlider;

        [Space(5)]
        [SerializeField] private Button _soundButton;
        [SerializeField] private Image _soundImage;
        [SerializeField] private Sprite _enabledSound;
        [SerializeField] private Sprite _disabledSound;

        [Header("SFX")]
        [SerializeField] private Slider _sfxSlider;

        [Space(5)]
        [SerializeField] private Button _sfxButton;
        [SerializeField] private Image _sfxImage;
        [SerializeField] private Sprite _enabledSFX;
        [SerializeField] private Sprite _disabledSFX;

        [Header("Controls Radio buttons")]
        [SerializeField] private Button _dragControlButton;
        [SerializeField] private Button _buttonsControlButton;

        [Space(5)]
        [SerializeField] private Sprite _activeRadioButton;
        [SerializeField] private Sprite _inactiveRadioButton;

        private CompositeDisposable _disposables;

        protected override void OnBind(SettingsViewModel viewModel)
        {
            viewModel.SoundVolume.Subscribe(e => _soundSlider.value = e).Dispose();
            viewModel.SFXVolume.Subscribe(e => _sfxSlider.value = e).Dispose();

            _disposables = new ()
            {
                viewModel.IsOpened.Subscribe(SetSettingsWindowActive),
                viewModel.IsSoundEnabled.Subscribe(ChangeSoundSprite),
                viewModel.IsSFXEnabled.Subscribe(ChangeSFXSprite),
                viewModel.CurrentControlIndex.Subscribe(ChangeRadioButtonsSprite)
            };

            BindControls();
        }

        private void SetSettingsWindowActive(bool isActive)
        {
            if (isActive)
                Show();
            else
                Close();
        }

        private void ChangeSoundSprite(bool isEnabled)
        {
            if (isEnabled)
            {
                _soundImage.sprite = _enabledSound;
            }
            else
            {
                _soundImage.sprite = _disabledSound;
            }
        }

        private void ChangeSFXSprite(bool isEnabled)
        {
            if (isEnabled)
            {
                _sfxImage.sprite = _enabledSFX;
            }
            else
            {
                _sfxImage.sprite = _disabledSFX;
            }
        }

        private void ChangeRadioButtonsSprite(int activeIndex)
        {
            if (activeIndex == ControlsIndex.Drag)
            {
                _dragControlButton.image.sprite = _activeRadioButton;
                _buttonsControlButton.image.sprite = _inactiveRadioButton;
            }
            else if (activeIndex == ControlsIndex.Buttons)
            {
                _dragControlButton.image.sprite = _inactiveRadioButton;
                _buttonsControlButton.image.sprite = _activeRadioButton;
            }
        }

        private void BindControls()
        {
            _closeButton.onClick.AddListener(() => _viewModel.SetSettingsWindowActive(false));
            _soundSlider.onValueChanged.AddListener(_viewModel.SetSoundVolume);
            _sfxSlider.onValueChanged.AddListener(_viewModel.SetSFXVolume);
            _soundButton.onClick.AddListener(_viewModel.SwitchSoundEnabled);
            _sfxButton.onClick.AddListener(_viewModel.SwitchSFXEnabled);
            _dragControlButton.onClick.AddListener(_viewModel.SetDragControl);
            _buttonsControlButton.onClick.AddListener(_viewModel.SetButtonsControl);
        }

        private void UnbindControls()
        {
            _closeButton.onClick.RemoveAllListeners();
            _soundSlider.onValueChanged.RemoveAllListeners();
            _sfxSlider.onValueChanged.RemoveAllListeners();
            _soundButton.onClick.RemoveAllListeners();
            _sfxButton.onClick.RemoveAllListeners();
            _dragControlButton.onClick.RemoveAllListeners();
            _buttonsControlButton.onClick.RemoveAllListeners();
        }

#region MonoBehaviour
        private void OnDestroy()
        {
            _disposables.Dispose();
            UnbindControls();
        }
#endregion
    }
}