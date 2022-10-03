using WaldemGame.Managers;

namespace WaldemGame.UI.Controller.ToggleController
{
    public class MusicToggleController : AToggleController
    {
        protected override void SubscribedAction(bool value)
        {
            App.GetManager<AudioManager>().SetMusic(value);
        }
    }
}