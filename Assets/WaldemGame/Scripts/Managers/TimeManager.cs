using UnityEngine;
using Waldem.Unity.Events;
using WaldemGame.Abstract;
using WaldemGame.UI.Controller.ButtonController;

namespace WaldemGame.Managers
{
    public class TimeManager : AManager
    {
    #pragma warning disable 0649
        [SerializeField]
        private TimerController Timer;
        [SerializeField]
        private TimeController Time;
    #pragma warning restore 0649

        [HideInInspector]
        public ClassicEvent OnSecondPassed = new ClassicEvent();
        public TimerController TimerController => Timer;

        public override void Init()
        {
            if(!Inited)
            {
                var gameManager = App.GetManager<GameManager>();
                gameManager.OnGameStart.AddListener(SetTimeByTimer);


                //uncomment if need to stop time on pause
                // gameManager.OnPause.AddListener(TimeController.StopTiming);
                // gameManager.OnResume.AddListener(TimeController.StartTiming);
                
                if(Time)
                {
                    var UIManager = App.GetManager<UIManager>();
                    UIManager.OnGameHUDShown.AddListener(Time.StartTiming);
                    
                    Time.Init();

                    Time.OnTimeUp.AddListener(TimeIsUp);
                }
                Inited = true;
            }
        }

        private void SetTimeByTimer()
        {
            Time.TimeInSeconds = Timer.CurrentSeconds;
        }

        private void TimeIsUp()
        {
            var gameManager = App.GetManager<GameManager>();
            gameManager.WellDone();
        }
    }
}