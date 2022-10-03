using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using WaldemGame.Managers;

namespace WaldemGame.UI.Menu
{
    public class InGamePauseMenu : AMenu
    {
        protected override void Subscribe()
        {
            var gameManager = App.GetManager<GameManager>();

            gameManager.OnPause.AddListener((pauseMenuType)=> 
            {
                if(pauseMenuType == PauseMenuTypes.InGame)
                {
                    ShowThroughManager();
                }
            });

            gameManager.OnResume.AddListener((pauseMenuType)=> 
            {
                if(pauseMenuType == PauseMenuTypes.InGame)
                {
                    HideThroughManager();
                }
            });

            gameManager.OnMenu.AddListener(HideThroughManager);
        }
    }
}