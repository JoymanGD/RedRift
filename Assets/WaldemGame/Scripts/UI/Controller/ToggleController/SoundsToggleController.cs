using WaldemGame.Managers;

namespace WaldemGame.UI.Controller.ToggleController
{
    public class SoundsToggleController : AToggleController
    {
        protected override void SubscribedAction(bool value)
        {
            App.GetManager<AudioManager>().SetSounds(value);
        }
    }
}