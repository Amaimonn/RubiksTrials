using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using R3;
using Assets._Game.Scripts.MVVM;
using Unity.VisualScripting;

namespace Assets._Game.Scripts.EntryPoint
{
    public class GameEntryPoint
    {
        private static GameEntryPoint _instance;
        private Coroutines _coroutines;
        private UIManager _uiManager;

        public GameEntryPoint()
        {
            _coroutines = new GameObject("[COROUTINES]").AddComponent<Coroutines>();
            Object.DontDestroyOnLoad(_coroutines.gameObject);

            var uiManagerPrefab = Resources.Load<UIManager>("UIManager");
            _uiManager = Object.Instantiate(uiManagerPrefab);
            Object.DontDestroyOnLoad(_uiManager);
            ServiceLocator.Current.Register(_uiManager);

            var prefabSoundPlayer = Resources.Load<SoundPlayer>("SoundPlayer");
            var soundPlayer = Object.Instantiate(prefabSoundPlayer);
            Object.DontDestroyOnLoad(soundPlayer);
            ServiceLocator.Current.Register(soundPlayer);

            var gameStateProvider = new DummyGameStateProvider();
            gameStateProvider.LoadSettings();
            ServiceLocator.Current.Register<IGameStateProvider>(gameStateProvider);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutostartGame()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            _instance = new GameEntryPoint();
            _instance.Run();

        }

        private void Run()
        {
#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == Scenes.TEST)
            {
                // LoadScene(sceneName);
                return;
            }
#endif
            _coroutines.StartCoroutine(LoadAndStartMainMenu());
        }

        private IEnumerator LoadScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
        }

        private IEnumerator LoadAndStartMainMenu(EnterMainMenuParameters enterMainMenuParameters = null)
        {   
            _uiManager.ShowLoadingScreen();

            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(Scenes.MAIN_MENU);

            yield return new WaitForSeconds(1);
            
            var sceneEntryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();

            sceneEntryPoint.Boot(enterMainMenuParameters).Subscribe(mainMenuExitParams =>
            {
                //var targetSceneName = mainMenuExitParams.TargetSceneEnterParams.SceneName;

                //if (targetSceneName == Scenes.GAMEPLAY)
                
                _coroutines.StartCoroutine(WithTransition(LoadAndStartGameplay(mainMenuExitParams)));
                
            });
            
            _uiManager.HideLoadingScreen();
        }

        private IEnumerator WithTransition(IEnumerator loadCoroutine)
        {
            yield return _uiManager.InTransition();
            yield return loadCoroutine;
            yield return _uiManager.OutTransition();
        }

        private IEnumerator LoadAndStartGameplay(EnterGameplayParameters enterGameplayParameters)
        {
            _uiManager.ShowLoadingScreen();

            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(Scenes.GAMEPLAY);

            yield return new WaitForSeconds(1);
            
            var sceneEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();

            sceneEntryPoint.Boot(enterGameplayParameters).Subscribe(gameplayExitParameters =>
            {
                //var targetSceneName = mainMenuExitParams.TargetSceneEnterParams.SceneName;

                //if (targetSceneName == Scenes.GAMEPLAY)
                
                _coroutines.StartCoroutine(WithTransition(LoadAndStartMainMenu(gameplayExitParameters)));
                
            });
            
            _uiManager.HideLoadingScreen();
        }
    }
}