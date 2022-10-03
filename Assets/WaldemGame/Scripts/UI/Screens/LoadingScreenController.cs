using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using WaldemGame.Abstract;
using WaldemGame.Managers;
using System.Threading.Tasks;

namespace WaldemGame.UI.Screens
{
    public class LoadingScreenController : MonoBehaviour, IInitable
    {
    #pragma warning disable 0649
    #pragma warning disable 0414
        [Header("Defining")]
        [SerializeField]
        private Image blocker;

        [Space]
        [Header("Data")]
        [SerializeField]
        private float fadingDuration;

    #pragma warning restore 0414
    #pragma warning restore 0649

        public bool Inited { get; set; }

        public void Init()
        {
            if(!Inited)
            {
                var gameManager = App.GetManager<GameManager>();
                gameManager.OnLoadingStart.AddListener(ShowLoadingScreen);
                Inited = true;
            }
        }

        public void Reinit()
        {
            Inited = false;
            Init();
        }

        public void ShowLoadingScreen()
        {
            gameObject.SetActive(true);
            blocker.DOFade(0, fadingDuration);
        }

        public void HideLoadingScreen(Action callBack = null)
        {
            blocker.DOFade(1, fadingDuration).OnComplete(()=> 
            {
                callBack?.Invoke();
                gameObject.SetActive(false);
            });
        }

        public void AsyncInit()
        {
        }
    }
}