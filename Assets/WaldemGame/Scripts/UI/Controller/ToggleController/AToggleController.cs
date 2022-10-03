using UnityEngine;
using UnityEngine.UI;

namespace WaldemGame.UI.Controller.ToggleController
{
    public abstract class AToggleController : MonoBehaviour
    {
    #pragma warning disable 0649
        [SerializeField]
        protected Toggle toggleComponent;
    #pragma warning restore 0649
        
        void Start()
        {
            toggleComponent.onValueChanged.AddListener(OnValueChangedHandler);
            Init();
        }

        private void OnValueChangedHandler(bool value)
        {
            SubscribedAction(value);
        }

        protected virtual void Init()
        {
        }

        protected void Toggle(bool value)
        {
            toggleComponent.isOn = value;
        }

        protected abstract void SubscribedAction(bool value);
    }
}