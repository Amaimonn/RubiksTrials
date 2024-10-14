using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets._Game.Scripts.MVVM.Abstractions;
using System.Collections;

namespace Assets._Game.Scripts.MVVM
{
    public class UIManager : MonoBehaviour, IService
    {
        // private IEnumerable<BaseScreen<IViewModel>> _screens;
        public float TransitionDelay => _transitionTime;
        [SerializeField] private RectTransform _canvasRectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _groupAnimationTime = 0.5f;
        [SerializeField] private RectTransform _transitionScreen;
        [SerializeField] private float _transitionTime = 1.0f;
        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private Transform _uiSceneContainer;
        private Dictionary<Type, AbstractScreen> _screensMap;
        private Dictionary<Type, AbstractScreen> _shownScreens;


        private void Awake()
        {
            HideLoadingScreen();
            _transitionScreen.gameObject.SetActive(false);
        }

        public void ShowLoadingScreen()
        {
            _loadingScreen.SetActive(true);
        }
        
        public void HideLoadingScreen()
        {
            _loadingScreen.SetActive(false);
        }

        public IEnumerator InTransition()
        {
            _canvasGroup.interactable = false;
            _transitionScreen.gameObject.SetActive(true);
            var currentLocalPosition = _transitionScreen.anchoredPosition;
            float eclapsedTime = 0.0f;
            while (eclapsedTime < _transitionTime)
            {
                currentLocalPosition.y = Mathf.Lerp(_canvasRectTransform.rect.height, -_canvasRectTransform.rect.height/2, 
                    eclapsedTime/_transitionTime);
                _transitionScreen.anchoredPosition = currentLocalPosition;
                eclapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        public IEnumerator OutTransition()
        {
            var currentLocalPosition = _transitionScreen.anchoredPosition;
            float eclapsedTime = 0.0f;
            while (eclapsedTime < _transitionTime)
            {
                currentLocalPosition.y = Mathf.Lerp(-_canvasRectTransform.rect.height/2, _canvasRectTransform.rect.height, 
                    eclapsedTime/_transitionTime);
                _transitionScreen.anchoredPosition = currentLocalPosition;
                eclapsedTime += Time.deltaTime;
                yield return null;
            }
            _transitionScreen.gameObject.SetActive(false);
            _canvasGroup.interactable = true;
        }

        public IEnumerator ShowGroupAnimation()
        {
            float eclapsedTime = 0.0f;
            while(eclapsedTime < _groupAnimationTime)
            {
                _canvasGroup.alpha = Mathf.Lerp(0,1, eclapsedTime/_groupAnimationTime);
                eclapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        public void AttachSceneUI(GameObject sceneUI)
        {
            ClearSceneUI();
            
            AddSceneUI(sceneUI);
        }

        public void AddSceneUI(GameObject sceneUI)
        {
            sceneUI.transform.SetParent(_uiSceneContainer, false);
        }

        // public void AttachSceneUI<T>(IEnumerable<T> sceneUIs) where T : MonoBehaviour
        // {
        //     ClearSceneUI();
        //     foreach (var sceneUI in sceneUIs)
        //     {
        //         sceneUI.transform.SetParent(_uiSceneContainer, false);
        //     }
        // }

        private void ClearSceneUI()
        {
            var childCount = _uiSceneContainer.childCount;
            for (var i = 0; i < childCount; i++)
            {
                Destroy(_uiSceneContainer.GetChild(i).gameObject);
            }
        }
        
        public void Initialize(IEnumerable<AbstractScreen> screens)
        {
            _shownScreens = new();
            foreach (var screen in screens)
            {
                screen.Close();
            }

            _screensMap = screens.ToDictionary(e => e.ViewModelType, e => e);
        }

        public void BindAndShow<TViewModel>(TViewModel model) where TViewModel : IViewModel
        {
            if (_screensMap.TryGetValue(model.ViewModelType, out var screen))
            {
                screen.Bind(model);
                screen.Show();
                _shownScreens.Add(model.ViewModelType, screen);
            }
            else
            {
                Debug.Log($"viewModel {typeof(TViewModel).Name} not found");
            }
        }

        public void BindHidden<TViewModel>(TViewModel model) where TViewModel : IViewModel
        {
            if (_screensMap.TryGetValue(model.ViewModelType, out var screen))
            {
                screen.Bind(model);
                _shownScreens.Add(model.ViewModelType, screen);
            }
            else
            {
                Debug.Log($"viewModel {typeof(TViewModel).Name} not found");
            }
        }

        public void ShowView<TViewModel>() where TViewModel : IViewModel
        {
            if (_screensMap.TryGetValue(typeof(TViewModel), out var screen))
            {
                screen.Show();
            }
            else
            {
                Debug.Log($"viewModel {typeof(TViewModel).Name} not found");
            }
        }

        public void Hide<TViewModel>() where TViewModel : IViewModel
        {
            if (_shownScreens.TryGetValue(typeof(TViewModel), out var screen))
            {
                screen.Dispose();
                screen.Close();
                _shownScreens.Remove(typeof(TViewModel));
            }
        }

        public void Hide<TViewModel>(TViewModel viewModel) where TViewModel : IViewModel
        {
            if (_shownScreens.TryGetValue(viewModel.ViewModelType, out var screen))
            {
                screen.Dispose();
                screen.Close();
                _shownScreens.Remove(viewModel.ViewModelType);
            }
        }
    }
}