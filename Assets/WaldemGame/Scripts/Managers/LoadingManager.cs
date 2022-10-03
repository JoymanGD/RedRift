using DG.Tweening;
using UnityEngine;
using WaldemGame.Abstract;
using WaldemGame.UI.Screens;

namespace WaldemGame.Managers
{    
    public class LoadingManager : AManager
    {
    #pragma warning disable 0649
        [SerializeField]
        private LogoScreenController logoScreenController;
        [SerializeField]
        private LoadingScreenController loadingScreenController;
        [SerializeField]
        private float fakeLoadingTime = 3;
    #pragma warning restore 0649

        public override void Init()
        {
            if(!Inited)
            {
                var gameManager = App.GetManager<GameManager>();
                gameManager.OnLogoStart.AddListener(()=> StartLogo(gameManager));
                gameManager.OnLogoEnd.AddListener(()=> StartLoading(gameManager));
                
                logoScreenController.gameObject.SetActive(true);
                loadingScreenController.gameObject.SetActive(true);
                loadingScreenController.Init();
                Inited = true;
            }
        }

        private void StartLogo(GameManager gameManager)
        {
            logoScreenController.PlayLogo(gameManager.LogoEnd);
        }

        private void StartLoading(GameManager gameManager)
        {
            gameManager.LoadingStart();
            App.Instance.StartInitializing();
            DOVirtual.DelayedCall(fakeLoadingTime, EndLoading);
        }

        public void Sync(float progress)
        {
            if(progress >= 0.99999f)
            {
            }
        }

        private void EndLoading()
        {
            loadingScreenController.HideLoadingScreen(()=>
            {
                var gameManager = App.GetManager<GameManager>();
                gameManager.LoadingEnd();
            });
        }
    }
}