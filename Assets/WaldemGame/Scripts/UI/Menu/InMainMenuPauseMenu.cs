using DG.Tweening;
using WaldemGame.Managers;

namespace WaldemGame.UI.Menu
{
    public class InMainMenuPauseMenu : AMenu
    {
        protected override void Subscribe()
        {
            var gameManager = App.GetManager<GameManager>();

            gameManager.OnPause.AddListener((pauseMenuType)=> 
            {
                if(pauseMenuType == PauseMenuTypes.InMainMenu)
                {
                    ShowThroughManager();
                }
            });

            gameManager.OnResume.AddListener((pauseMenuType)=> 
            {
                if(pauseMenuType == PauseMenuTypes.InMainMenu)
                {
                    HideThroughManager();
                }
            });

            gameManager.OnMenu.AddListener(HideThroughManager);
        }
    }
}