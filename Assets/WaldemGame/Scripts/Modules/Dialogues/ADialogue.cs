using Waldem.Unity.Events;

namespace WaldemGame.Dialogues
{
    public abstract class ADialogue
    {
        public ClassicEvent OnDialogueStarted = new ClassicEvent();
        public ClassicEvent OnDialogueEnded = new ClassicEvent();
        
        public abstract string StartDialogue();
        public abstract string Next();
    }
}