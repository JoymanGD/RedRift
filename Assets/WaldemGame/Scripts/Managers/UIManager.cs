using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using Waldem.Unity.Events;
using WaldemGame.UI.HUD;
using WaldemGame.UI.Menu;
using WaldemGame.UI;
using TMPro;

namespace WaldemGame.Managers
{    
    public class UIManager : AManager
    {
    #pragma warning disable 0649
        [SerializeField]
        private BlackBlockerController worldBlocker;
        [SerializeField]
        private BlackBlockerController menuBlocker;
        [SerializeField]
        private BlackBlockerController hudBlocker;
        [SerializeField]
        private float menuBlockerFadeDuration;
        [SerializeField]
        private HUD GameHUD;
        [SerializeField]
        private HUD MenuHUD;
        [SerializeField]
        private TextMeshProUGUI tipText;

        [SerializeField]
        private AMenu[] Menus;
    #pragma warning restore 0649

        private Sequence mainHUDSequence;
        private AMenu openedMenu;

        public ClassicEvent OnGameHUDShown = new ClassicEvent();

        public override void Init()
        {
            if(!Inited)
            {
                var gameManager = App.GetManager<GameManager>();
                gameManager.OnGameStart.AddListener(ShowGameHUDHandler);
                gameManager.OnMenu.AddListener(ShowMenuHUDHandler);
                gameManager.OnAppEnter.AddListener(ShowGameScreen);
                // gameManager.OnAppEnter.AddListener(ShowMenuHUDFirstTimeHandler);

                worldBlocker?.gameObject.SetActive(true);
                menuBlocker?.gameObject.SetActive(true);
                hudBlocker?.gameObject.SetActive(true);
                GameHUD?.gameObject.SetActive(true);
                MenuHUD?.gameObject.SetActive(true);

                GameHUD?.Init();
                MenuHUD?.Init();

                mainHUDSequence = DOTween.Sequence();

                foreach (var item in Menus)
                {
                    item?.Init();
                }
                Inited = true;
            }
        }

        public void ShowGameScreen()
        {
            worldBlocker.Hide(1);
            hudBlocker.Hide(1);
        }

        public void HideGameScreen(Action callBack = null)
        {
            worldBlocker.Show(1, callBack: callBack);
            hudBlocker.Show(1);
        }

        public void ShowTip(string text)
        {
            if(tipText)
            {
                tipText.gameObject.SetActive(true);
                tipText.text = text;
            }
        }

        public void HideTip()
        {
            if(tipText)
            {
                tipText.gameObject.SetActive(false);
                tipText.text = "";
            }
        }

        private void ShowGameHUDHandler()
        {
            mainHUDSequence.Complete();

            var menuHUDHideTween = MenuHUD.GetHideTween();
            var gameHUDShowTween = GameHUD.GetShowTween();

            mainHUDSequence = DOTween.Sequence().SetAutoKill(false);

            mainHUDSequence
                .Append(menuHUDHideTween)
                .Append(gameHUDShowTween)
                .AppendCallback(()=> OnGameHUDShown?.Invoke());

            mainHUDSequence.Play();
        }

        private void ShowMenuHUDHandler()
        {
            mainHUDSequence.Complete();
            
            var gameHUDHideTween = GameHUD.GetHideTween();
            var menuHUDShowTween = MenuHUD.GetShowTween();

            mainHUDSequence = DOTween.Sequence().SetAutoKill(false);

            mainHUDSequence
                .Append(gameHUDHideTween)
                .Append(menuHUDShowTween);

            mainHUDSequence.Play();
        }

        private void ShowMenuHUDFirstTimeHandler()
        {
            var menuHUDShowTween = MenuHUD.GetShowTween();
            menuHUDShowTween.Play();
        }

        public void HideGameHUD()
        {
            GameHUD.GetHideTween().Play();
        }

        public void ShowMenu(AMenu menu, Action callBack = null)
        {
            if(!menu.IsShowing)
            {
                menu.IsShowing = true;

                menuBlocker.Show(menuBlockerFadeDuration, .95f);

                openedMenu = menu;

                menu.Show(callBack);
            }
        }

        public void ShowMenu<T>(Action callBack = null) where T : AMenu
        {
            var menu = Menus.FirstOrDefault(a=> a.GetType() == typeof(T));

            ShowMenu(menu, callBack);
        }

        public void ShowMenu<T>(GameStates checkState, Action callBack = null) where T : AMenu
        {
            if(CheckForGameState(checkState))
            {
                ShowMenu<T>(callBack);
            }
        }

        public void ShowMenu<T>(List<GameStates> values, Action callBack = null) where T : AMenu
        {
            if(CheckForGameState(values))
            {
                ShowMenu<T>(callBack);
            }
        }

        public void HideMenu(AMenu menu, Action callBack = null){
            if(menu.IsShowing)
            {
                menuBlocker.Hide(menuBlockerFadeDuration);

                if(menu == openedMenu)
                {
                    openedMenu = null;
                }

                menu.Hide(()=>
                {
                    callBack?.Invoke();
                    menu.IsShowing = false;
                });
            }
        }

        public void HideOpenedMenu()
        {
            if(openedMenu)
            {
                HideMenu(openedMenu);
            }
        }

        public void HideMenu<T>(Action callBack = null)
        {
            var menu = Menus.FirstOrDefault(a=> a.GetType() == typeof(T));

            HideMenu(menu, callBack);
        }

        public T GetMenu<T>() where T : AMenu
        {
            var menu = Menus.FirstOrDefault(e=> e.GetType() == typeof(T));
            return (T)menu;
        }

        private bool CheckForGameState(GameStates value)
        {
            var gameManager = App.GetManager<GameManager>();
            return gameManager.CurrentGameState == value;
        }

        private bool CheckForGameState(List<GameStates> values)
        {
            var gameManager = App.GetManager<GameManager>();

            foreach (var item in values)
            {
                if(gameManager.CurrentGameState == item)
                {
                    return true;
                }
            }

            return false;
        }

        public static Tween GetUIElementHideAnimation(RectTransform elementRectTransform, UIElementAnimationSides animationSide, float duration, Action callBack = null)
        {
            elementRectTransform.DOKill(true);

            var position = elementRectTransform.position;
            var height = elementRectTransform.sizeDelta.y * elementRectTransform.lossyScale.y;
            var width = elementRectTransform.sizeDelta.x * elementRectTransform.lossyScale.x;
            var screenWidth = Screen.width;
            var screenHeight = Screen.height;
            Vector3 endValue = position;
            float x, y;

            switch (animationSide)
            {
                case UIElementAnimationSides.Up:
                    y = screenHeight + height/2;
                    endValue = new Vector3(position.x, y, position.z);
                    break;
                case UIElementAnimationSides.Down:
                    y = -height/2;
                    endValue = new Vector3(position.x, y, position.z);
                    break;
                case UIElementAnimationSides.Right:
                    x = screenWidth + width/2;
                    endValue = new Vector3(x, position.y, position.z);
                    break;
                case UIElementAnimationSides.Left:
                    x = -width/2;
                    endValue = new Vector3(x, position.y, position.z);
                    break;
            }

            Tween hideTween = elementRectTransform.DOMove(endValue, duration).OnComplete(()=> 
            {
                elementRectTransform.position = position;
                callBack?.Invoke();
                elementRectTransform.gameObject.SetActive(false);
            });

            return hideTween;
        }

        public static Tween GetUIElementShowAnimation(RectTransform elementRectTransform, UIElementAnimationSides animationSide, float duration, Action callBack = null)
        {
            elementRectTransform.DOKill(true);
            
            var position = elementRectTransform.position;
            var height = elementRectTransform.sizeDelta.y * elementRectTransform.lossyScale.y;
            var width = elementRectTransform.sizeDelta.x * elementRectTransform.lossyScale.x;
            var screenWidth = Screen.width;
            var screenHeight = Screen.height;
            
            Vector3 hideValue = position;
            float x, y;

            switch (animationSide)
            {
                case UIElementAnimationSides.Up:
                    y = screenHeight + height/2;
                    hideValue = new Vector3(position.x, y, position.z);
                    break;
                case UIElementAnimationSides.Down:
                    y = -height/2;
                    hideValue = new Vector3(position.x, y, position.z);
                    break;
                case UIElementAnimationSides.Right:
                    x = screenWidth + width/2;
                    hideValue = new Vector3(x, position.y, position.z);
                    break;
                case UIElementAnimationSides.Left:
                    x = -width/2;
                    hideValue = new Vector3(x, position.y, position.z);
                    break;
            }

            elementRectTransform.position = hideValue;

            Tween hideTween = elementRectTransform.DOMove(position, duration)
                                                        .OnStart(()=>
                                                        {
                                                            elementRectTransform.gameObject.SetActive(true);
                                                        })
                                                        .OnComplete(()=>
                                                        {
                                                            callBack?.Invoke();
                                                        });

            return hideTween;
        }
    }

    public enum UIElementAnimationSides
    {
        Up, Down, Left, Right
    }

    public enum PauseMenuTypes
    {
        InGame, InMainMenu, Any
    }
}