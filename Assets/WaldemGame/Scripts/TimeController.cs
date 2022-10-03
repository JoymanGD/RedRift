using System;
using System.Collections;
using System.Globalization;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Waldem.Unity.Events;
using WaldemGame.Abstract;
using WaldemGame.Managers;

namespace WaldemGame
{
    public class TimeController : MonoBehaviour, IInitable
    {
    #pragma warning disable 0649
        [SerializeField]
        private TextMeshProUGUI TimeText;
    #pragma warning restore 0649

        private IEnumerator timingCoroutine;

        public int TimeInSeconds { get => GetTimeInSeconds(); set => SetTimeWithSeconds(value); }
        public bool Inited { get; set; } = false;
        public ClassicEvent OnTimeUp = new ClassicEvent();

        public void Init()
        {
            if(!Inited)
            {
                Inited = true;
            }
        }

        public void Reinit()
        {
            Inited = false;
            Init();
        }

        public void StartTiming()
        {
            timingCoroutine = SecondPassed();
            StartCoroutine(timingCoroutine);
        }

        public void StopTiming()
        {
            StopCoroutine(timingCoroutine);
        }

        private IEnumerator SecondPassed()
        {
            ReduceTime(1);
            
            App.GetManager<AudioManager>().PlaySound("SecondPassed");

            yield return new WaitForSeconds(1);

            timingCoroutine = SecondPassed();

            StartCoroutine(timingCoroutine);
        }

        public void ReduceTime(int seconds)
        {
            var timeInSeconds = TimeInSeconds;
            
            if(timeInSeconds > 0)
            {
                timeInSeconds -= seconds;
                SetTimeWithSeconds(timeInSeconds);
            }
            else{
                TimeIsUpHandler();
            }
        }

        public void AddTime(int seconds)
        {
            TimeInSeconds += seconds;
        }

        private void TimeIsUpHandler()
        {
            var gameManager = App.GetManager<GameManager>();
            gameManager.HideAllMenus();

            OnTimeUp?.Invoke();
        }

        private void SetTimeWithSeconds(int seconds)
        {
            TimeSpan t = TimeSpan.FromSeconds(seconds);

            string result = $"{ZeroFormatTime(t.Minutes)}:{ZeroFormatTime(t.Seconds)}";

            TimeText.text = result;
        }

        /// <summary>
        /// Prepends "0" if inner value < 10
        /// </summary>
        /// <param name="value">Inner value</param>
        /// <returns>string</returns>
        private string ZeroFormatTime(int value)
        {
            if(value < 10)
            {
                return $"0{value}";
            }

            return $"{value}";
        }

        private int GetTimeInSeconds()
        {
            TimeSpan t = TimeSpan.ParseExact(TimeText.text, @"mm\:ss", CultureInfo.InvariantCulture, TimeSpanStyles.None);

            return (int)t.TotalSeconds;
        }

        public void AsyncInit()
        {
        }
    }
}