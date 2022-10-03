using UnityEngine;
using WaldemGame.Managers;

namespace WaldemGame.UI.Controller.ButtonController
{
    public class PauseButtonController : AButtonController
    {
    #pragma warning disable 0649
        [SerializeField]
        private PauseButtonTypes Type;
    #pragma warning restore 0649

        protected override void SubscribedAction()
        {
            switch(Type)
            {
                case PauseButtonTypes.Pause:
                    PauseHandler();
                    break;
                case PauseButtonTypes.Resume:
                    ResumeHandler();
                    break;
            }
        }

        private void PauseHandler()
        {
            App.GetManager<GameManager>().Pause();
        }

        private void ResumeHandler()
        {
            App.GetManager<GameManager>().Resume();
        }

        public enum PauseButtonTypes
        {
            Pause, Resume
        }
    }
}