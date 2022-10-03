using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using WaldemGame.Managers;

namespace WaldemGame.UI.Controller.ButtonController
{
    public abstract class AButtonController : MonoBehaviour
    {
    #pragma warning disable 0649
        [SerializeField]
        protected Button buttonComponent;
    #pragma warning restore 0649
        
        void Start()
        {
            Init();
            buttonComponent.onClick.AddListener(OnClickHandler);
        }

        protected virtual void Init()
        {
        }

        private void OnClickHandler()
        {
            App.GetManager<AudioManager>().PlaySound("ButtonClick_01");

            //animation
            var _transform = buttonComponent.transform;
            var punchForce = - (_transform.localScale * 0.1f);
            _transform.DOPunchScale(punchForce, .2f, 1);

            SubscribedAction();
        }

        protected abstract void SubscribedAction();
    }    
}