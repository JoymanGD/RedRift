using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

namespace WaldemGame.UI
{
    public class BlackBlockerController : MonoBehaviour
    {
    #pragma warning disable 0649
        [SerializeField]
        private Image Blocker;
    #pragma warning restore 0649

        public void Show(float duration, float endValue = 1, Action callBack = null)
        {
            Blocker.DOComplete(true);
            
            Blocker.DOFade(endValue, duration).OnComplete(()=> {
                SetRaycasting(true);
                callBack?.Invoke();
            });
        }

        public void Hide(float duration, Action callBack = null)
        {
            Blocker.DOComplete(true);

            Blocker.DOFade(0, duration).OnComplete(()=> {
                SetRaycasting(false);
                callBack?.Invoke();
            });
        }

        public void SetRaycasting(bool value)
        {
            Blocker.raycastTarget = value;
        }
    }
}