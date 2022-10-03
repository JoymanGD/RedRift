using WaldemGame.Managers;

namespace WaldemGame.UI.Controller.ButtonController
{
    public class TryAgainButtonController : AButtonController
    {
        protected override void SubscribedAction()
        {
            RestartGame();
        }

        private void RestartGame()
        {
            App.GetManager<GameManager>().StartGame();
        }
    }
}