using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using WaldemGame.UI.Controller.ButtonController;

namespace WaldemGame.UI.Controller.ButtonController
{
    public class TimerController : AButtonController
    {
    #pragma warning disable 0649
        [SerializeField]
        private TextMeshProUGUI textComponent;
        [SerializeField]
        private int[] secondsList;
    #pragma warning restore 0649

        private int currentSeconds
        {
            get => TextToSeconds(textComponent.text);
            set => textComponent.text = SecondsToText(value);
        }
        private int index = 0;

        public int CurrentSeconds => currentSeconds;

        protected override void SubscribedAction()
        {
            if(secondsList != null && secondsList.Length > 0)
            {
                index++;

                if(index >= secondsList.Length)
                {
                    index = 0;
                }

                currentSeconds = secondsList[index];
            }
        }

        private int TextToSeconds(string text)
        {
            var timespan = TimeSpan.ParseExact(text, @"mm\:ss", CultureInfo.InvariantCulture, TimeSpanStyles.None);

            return (int)timespan.TotalSeconds;
        }

        private string SecondsToText(int seconds)
        {
            var timespan = TimeSpan.FromSeconds(seconds);

            return timespan.ToString(@"mm\:ss");
        }
    }
}