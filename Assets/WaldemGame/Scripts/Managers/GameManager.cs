using UnityEngine;
using Waldem.Unity.Events;
using WaldemGame.Abstract;

namespace WaldemGame.Managers
{    
    public class GameManager : AManager, IUpdatable, IFixedUpdatable, ILateUpdatable
    {

        [HideInInspector]
        public ClassicEvent OnLogoStart = new ClassicEvent();
        [HideInInspector]
        public ClassicEvent OnLogoEnd = new ClassicEvent();
        [HideInInspector]
        public ClassicEvent OnAppEnter = new ClassicEvent();
        [HideInInspector]
        public ClassicEvent OnAppPause = new ClassicEvent();
        [HideInInspector]
        public FloatEvent OnAppUpdate = new FloatEvent();
        [HideInInspector]
        public ClassicEvent OnAppQuit = new ClassicEvent();
        [HideInInspector]
        public ClassicEvent OnLoadingStart = new ClassicEvent();
        [HideInInspector]
        public ClassicEvent OnLoadingEnd = new ClassicEvent();
        [HideInInspector]
        public ClassicEvent OnGameStart = new ClassicEvent();
        [HideInInspector]
        public ClassicEvent OnGameEnd = new ClassicEvent();
        [HideInInspector]
        public ClassicEvent OnWellDone = new ClassicEvent();
        [HideInInspector]
        public ClassicEvent OnTryAgain = new ClassicEvent();
        [HideInInspector]
        public FloatEvent OnGameUpdate = new FloatEvent();
        [HideInInspector]
        public FloatEvent OnGameFixedUpdate = new FloatEvent();
        [HideInInspector]
        public FloatEvent OnGameLateUpdate = new FloatEvent();
        [HideInInspector]
        public ClassicEvent OnMenu = new ClassicEvent();
        [HideInInspector]
        public ClassicEvent OnTutorialEnter = new ClassicEvent();
        [HideInInspector]
        public ClassicEvent OnTutorialExit = new ClassicEvent();
        [HideInInspector]
        public ClassicEvent OnAllMenusHide = new ClassicEvent();
        [HideInInspector]
        public FloatEvent OnMenuUpdate = new FloatEvent();
        [HideInInspector]
        public FloatEvent OnMenuFixedUpdate = new FloatEvent();
        [HideInInspector]
        public FloatEvent OnMenuLateUpdate = new FloatEvent();
        [HideInInspector]
        public CustomEvent<PauseMenuTypes> OnPause = new CustomEvent<PauseMenuTypes>();
        [HideInInspector]
        public CustomEvent<PauseMenuTypes> OnResume = new CustomEvent<PauseMenuTypes>();
        [HideInInspector]
        public GameStates CurrentGameState { get => currentGameState; }

        private GameStates currentGameState;

        public override void Init()
        {
            if(!Inited)
            {
                LogoStart();
                Inited = true;
            }
        }

        public void DoUpdate(float deltaTime)
        {
            OnAppUpdate?.Invoke(deltaTime);

            switch (currentGameState)
            {
                case GameStates.Game:
                    OnGameUpdate?.Invoke(deltaTime);
                    break;
                case GameStates.Menu:
                    OnMenuUpdate?.Invoke(deltaTime);
                    break;
            }
        }

        public void DoFixedUpdate(float deltaTime)
        {
            switch (currentGameState)
            {
                case GameStates.Game:
                    OnGameFixedUpdate?.Invoke(deltaTime);
                    break;
                case GameStates.Menu:
                    OnMenuFixedUpdate?.Invoke(deltaTime);
                    break;
            }
        }

        public void DoLateUpdate(float deltaTime)
        {
            switch (currentGameState)
            {
                case GameStates.Game:
                    OnGameLateUpdate?.Invoke(deltaTime);
                    break;
                case GameStates.Menu:
                    OnMenuLateUpdate?.Invoke(deltaTime);
                    break;
            }
        }

        //calls when app is switching to another app or closed
        private void OnApplicationPause(bool pauseStatus)
        {
            if(pauseStatus)
            {
                OnAppPause?.Invoke();
            }
        }

        //calls in editor on game exit
        private void OnApplicationQuit()
        {
            OnAppQuit?.Invoke();
        }

        public void LogoStart()
        {
            currentGameState = GameStates.None;
            OnLogoStart?.Invoke();
        }

        public void LogoEnd()
        {
            OnLogoEnd?.Invoke();
        }

        public void LoadingStart()
        {
            OnLoadingStart?.Invoke();
            currentGameState = GameStates.Loading;
        }

        public void LoadingEnd()
        {
            OnLoadingEnd?.Invoke();
            AppStart();
        }

        public void AppStart()
        {
            OnAppEnter?.Invoke();
            currentGameState = GameStates.Menu;

            //TEMP
            StartGame();
        }

        public void StartGame()
        {
            OnGameStart?.Invoke();
            currentGameState = GameStates.Game;
        }

        public void WellDone()
        {
            OnWellDone?.Invoke();
        }

        public void TryAgain()
        {
            OnTryAgain?.Invoke();
            // currentGameState = GameStates.TryAgain;
        }

        public void Menu()
        {
            if(currentGameState == GameStates.Game)
            {
                OnGameEnd?.Invoke();
            }

            OnMenu?.Invoke();
            currentGameState = GameStates.Menu;
        }

        public void Pause()
        {
            PauseMenuTypes pauseMenuType = PauseMenuTypes.InGame;

            if(currentGameState == GameStates.Menu)
            {
                pauseMenuType = PauseMenuTypes.InMainMenu;
            }

            OnPause?.Invoke(pauseMenuType);
        }

        public void Resume()
        {
            PauseMenuTypes pauseMenuType = PauseMenuTypes.InGame;

            if(currentGameState == GameStates.Menu)
            {
                pauseMenuType = PauseMenuTypes.InMainMenu;
            }

            OnResume?.Invoke(pauseMenuType);
        }

        public void EnterTutorial()
        {
            OnTutorialEnter?.Invoke();
        }
        
        public void ExitTutorial()
        {
            OnTutorialExit?.Invoke();
        }

        public void HideAllMenus()
        {
            OnAllMenusHide?.Invoke();
        }
    }

    public enum GameStates
    {
        Game = 0,
        Menu = 1,
        Loading = 2,
        None = 3,
        TryAgain = 4
    }
}