using System;
using UnityEngine;

namespace WaldemGame.Dialogues
{
    [Serializable]
    public class SimpleDialogue : ADialogue
    {
        [SerializeField]
        [TextArea(3, 9)]
        private string[] talks;

        private int currentTalkIndex = 0;

        public override string StartDialogue()
        {
            OnDialogueStarted?.Invoke();

            currentTalkIndex = 0;

            return Next();
        }

        public override string Next()
        {
            if(talks.Length == 0)
            {
                Debug.LogError("There are no talks in dialogue");
                return "ERROR";
            }

            if(currentTalkIndex >= talks.Length)
            {
                return EndDialogue();
            }

            string currentTalk = talks[currentTalkIndex];

            currentTalkIndex++;

            return currentTalk;
        }

        private string EndDialogue()
        {
            OnDialogueEnded?.Invoke();

            var dialoguesManager = App.GetManager<DialoguesManager>();
            return dialoguesManager.EndDialogMark;
        }
    }
}