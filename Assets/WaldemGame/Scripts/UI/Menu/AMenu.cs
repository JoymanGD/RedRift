using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using WaldemGame.Abstract;
using WaldemGame.Managers;

namespace WaldemGame.UI.Menu
{
    public abstract class AMenu : MonoBehaviour, IShowable, IInitable
    {
    #pragma warning disable 0649
        [SerializeField]
        protected RectTransform root;
        [SerializeField]
        protected float showHideDuration = .3f;
    #pragma warning restore 0649

        public bool Inited { get; set; }
        public bool IsShowing { get; set; }

        public void Init()
        {
            if(!Inited)
            {
                gameObject.SetActive(true);
                
                var gameManager = App.GetManager<GameManager>();
                gameManager.OnAllMenusHide.AddListener(HideThroughManager);
                
                Subscribe();
                Inited = true;
            }
        }

        public void Reinit()
        {
            Inited = false;
            Init();
        }

        protected abstract void Subscribe();

        public virtual void Show(Action callBack = null)
        {
            PlayShowSound();

            var offsetY = Screen.height/2 + (root.sizeDelta.y/2 * root.localScale.y);
            
            var endYValue = root.position.y;

            root.position = root.position + new Vector3(0, offsetY, 0);

            root.gameObject.SetActive(true);

            if(DOTween.IsTweening(root))
            {
                root.DOKill(true);
            }

            root.DOMoveY(endYValue, showHideDuration).OnComplete(()=> callBack?.Invoke());
        }

        public virtual void Hide(Action callBack = null)
        {
            PlayHideSound();
            
            var offsetY = Screen.height/2 + (root.sizeDelta.y/2 * root.localScale.y);
            
            var tempPosition = root.position;
            
            var endYValue = root.position.y + offsetY;

            if(DOTween.IsTweening(root))
            {
                root.DOKill(true);
            }

            root.DOMoveY(endYValue, showHideDuration).OnComplete(()=> {
                callBack?.Invoke();
                root.position = tempPosition;
                root.gameObject.SetActive(false);
            });
        }

        protected virtual void PlayShowSound()
        {
            App.GetManager<AudioManager>().PlaySound("GameMenuSlideIn", .8f);
        }

        protected virtual void PlayHideSound()
        {
            App.GetManager<AudioManager>().PlaySound("GameMenuSlideOut", .8f);
        }

        protected void ShowThroughManager()
        {
            var UIManager = App.GetManager<UIManager>();
            UIManager.ShowMenu(this);
        }

        protected void HideThroughManager()
        {
            var UIManager = App.GetManager<UIManager>();
            UIManager.HideMenu(this);
        }

        public void AsyncInit()
        {
        }
    }
}