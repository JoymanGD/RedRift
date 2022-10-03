using DG.Tweening;
using WaldemGame.UI;
using TMPro;
using UnityEngine;
using WaldemGame.Managers;

namespace WaldemGame.Dialogues
{
    public class DialoguesPanelView : APanelView
    {
        [SerializeField]
        private RectTransform dialoguePanel;
        [SerializeField]
        private TextMeshProUGUI nameText;
        [SerializeField]
        private TextMeshProUGUI dialogueText;
        [SerializeField]
        private TextMeshProUGUI dialoguePanelTip;
        [SerializeField]
        private float textShowDurationPerChar = .05f;

        private Tween showTextTween;
        private const float SCREEN_TEXT_RATIO = 83.68f;

        public Tween ShowPanelAndSetTips(string name, float delay = 0)
        {
            var inputManager = App.GetManager<InputManager>();
            var talkKeycode = inputManager.GetKeyByInputName("Talk");
            dialoguePanelTip.text = $"Press <{talkKeycode}> to continue";

            nameText.text = name;

            return ShowPanel(delay);
        }

        public override Tween ShowPanel(float delay = 0)
        {
            return base.ShowPanel(delay);
        }

        public override Tween HidePanel(float delay = 0)
        {
            ClearText();

            return base.HidePanel(delay);
        }

        public bool IsShowingText()
        {
            return showTextTween.IsActive() && showTextTween.IsPlaying();
        }

        public void CompleteTextShowing()
        {
            if(showTextTween != null)
            {
                showTextTween.Complete(true);
            }
        }

        public Tween ShowText(string text)
        {
            var aspectRatio = (float)Screen.width/(float)Screen.height;
            dialogueText.fontSizeMax = SCREEN_TEXT_RATIO * aspectRatio;

            ClearText();

            dialogueText.text = text;
            dialogueText.maxVisibleCharacters = 0;
            var duration = textShowDurationPerChar * text.Length;

            showTextTween = DOTween.To(()=> dialogueText.maxVisibleCharacters, x => dialogueText.maxVisibleCharacters = x, text.Length, duration);

            showTextTween.SetEase(Ease.Linear);

            return showTextTween;
        }

        private void ClearText()
        {
            dialogueText.text = "";
        }
    }
}