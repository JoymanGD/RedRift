using DG.Tweening;
using WaldemGame.Inventory;
using UnityEngine;
using WaldemGame.Managers;

namespace WaldemGame.Dialogues
{
    public class DialoguesManager : AManager
    {
        [SerializeField]
        private DialoguesPanelView dialoguesPanelView;
        [SerializeField]
        private string endDialogMark = "<end_dialog>";

        private ADialogue currentDialogue;
        private bool dialogIsStarting;

        public string EndDialogMark => endDialogMark;

        public void StartDialogue(string name, ADialogue dialogue)
        {
            dialogIsStarting = true;

            var inventoryManager = App.GetManager<InventoryManager>();
            var inputManager = App.GetManager<InputManager>();
            inputManager.DisableInput(InputTypes.All);
            inputManager.DisableAllExceptOneInputButton(InputTypes.All, "Talk");

            inputManager.OnKeyPressed.RemoveListener(Continue);
            inputManager.OnKeyPressed.AddListener(Continue);

            currentDialogue = dialogue;

            var talk = currentDialogue.StartDialogue();

            var startDialogSequence = DOTween.Sequence();

                var inventoryHideTween = inventoryManager.HideInventoryPanel();
            
                var panelShowTween = dialoguesPanelView.ShowPanelAndSetTips(name);

                startDialogSequence
                            .Append(inventoryHideTween)
                            .Append(panelShowTween);

                startDialogSequence.AppendCallback(()=> OnDialogStartedHandler(talk));

                startDialogSequence.Play();
        }

        private void Continue(InputKeyData keyData)
        {
            if(dialogIsStarting)
            {
                return;
            }

            var inputManager = App.GetManager<InputManager>();
            var talkKeycode = inputManager.GetKeyByInputName("Talk");

            if(keyData.keyCode == talkKeycode)
            {
                if(dialoguesPanelView.IsShowingText())
                {
                    dialoguesPanelView.CompleteTextShowing();
                    return;
                }

                var talk = currentDialogue.Next();

                if(talk == endDialogMark)
                {
                    EndDialogue();
                    return;
                }

                dialoguesPanelView.ShowText(talk);
            }
        }

        private void OnDialogStartedHandler(string text)
        {
            dialogIsStarting = false;

            var showTextTween = dialoguesPanelView.ShowText(text);
            showTextTween.Play();
        }

        private void EndDialogue()
        {
            var inventoryManager = App.GetManager<InventoryManager>();
            var inputManager = App.GetManager<InputManager>();
            inputManager.OnKeyPressed.RemoveListener(Continue);
            inputManager.EnableInput(InputTypes.All);
            inputManager.EnableAllInputButtons(InputTypes.All);

            var endDialogSequence = DOTween.Sequence();

                var panelHideTween = dialoguesPanelView.HidePanel();
            
                var inventoryShowTween = inventoryManager.ShowInventoryPanel();

                endDialogSequence
                            .Append(panelHideTween)
                            .Append(inventoryShowTween);

                endDialogSequence.Play();
        }
    }
}