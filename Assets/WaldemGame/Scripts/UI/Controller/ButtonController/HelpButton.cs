using UnityEngine;
using WaldemGame.Managers;

namespace WaldemGame.UI.Controller.ButtonController
{
    public class HelpButton : AButtonController
    {
    #pragma warning disable 0649
        [SerializeField]
        private EnterExit enterExit;
    #pragma warning restore 0649

        protected override void SubscribedAction()
        {
            switch (enterExit)
            {
                case EnterExit.Enter:
                    App.GetManager<GameManager>().EnterTutorial();
                    break;
                case EnterExit.Exit:
                    App.GetManager<GameManager>().ExitTutorial();
                    break;
            }
        }
    }

    public enum EnterExit
    {
        Enter, Exit
    }
}