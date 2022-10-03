using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WaldemGame.Managers;

namespace WaldemGame
{
    public class UsableObject : MonoBehaviour
    {
        [SerializeField]
        private string inputName;
        [SerializeField]
        private string actionTextString;

        protected static List<UsableObject> objectsToUse = new List<UsableObject>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggerEnterAction(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            OnTriggerExitAction(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            OnTriggerStayAction(other);
        }

        protected virtual void OnTriggerEnterAction(Collider2D other)
        {
            StartUsing();
        }

        protected virtual void OnTriggerExitAction(Collider2D other)
        {
            StopUsing();
        }

        protected void StartUsing()
        {
            objectsToUse.Add(this);

            var inputManager = App.GetManager<InputManager>();
            inputManager.OnKeyPressed.RemoveListener(OnKeyPressedHandler);
            inputManager.OnKeyPressed.AddListener(OnKeyPressedHandler);

            var uiManager = App.GetManager<UIManager>();
            var button = inputManager.GetKeyByInputName(inputName);
            
            uiManager.ShowTip($"Press <{button}> to {actionTextString}");
        }

        protected void StopUsing()
        {
            objectsToUse.Remove(this);
        
            var inputManager = App.GetManager<InputManager>();
            inputManager.OnKeyPressed.RemoveListener(OnKeyPressedHandler);

            HidePlayerTip();
        }

        protected virtual void HidePlayerTip()
        {
            if(objectsToUse.Count == 0)
            {
                var uiManager = App.GetManager<UIManager>();
                uiManager.HideTip();
            }
        }

        protected virtual void OnTriggerStayAction(Collider2D other)
        {
        }

        private void OnKeyPressedHandler(InputKeyData inputData)
        {
            if(inputData.Name == inputName)
            {
                if(objectsToUse.Last() == this)
                {
                    UsableAction();
                }
            }
        }

        protected virtual void UsableAction()
        {

        }
    }
}