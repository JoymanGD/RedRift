using WaldemGame.Managers;

namespace WaldemGame.UI.Controller.ButtonController
{
    public class MainMenuButtonController : AButtonController
    {
        protected override void SubscribedAction()
        {
            MainMenuHandler();
        }

        private void MainMenuHandler()
        {
            App.GetManager<GameManager>().Menu();
        }
    }
}