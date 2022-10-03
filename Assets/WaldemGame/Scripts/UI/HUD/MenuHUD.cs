using System;
using DG.Tweening;
using UnityEngine;

namespace WaldemGame.UI.HUD
{
    public class MenuHUD : HUD
    {
    #pragma warning disable 0649
        [Header("Difining")]

        [Header("Tween settings")]
        [SerializeField]
        private float ElementsHideDuration;
        [SerializeField]
        private float ElementsHideInterval;
    #pragma warning restore 0649

        protected Sequence mainSequence { get; set; }

        protected override void InternalInit()
        {
            mainSequence = DOTween.Sequence();
        }

        public override Tween GetHideTween(Action callBack = null)
        {
            mainSequence.Complete(true);

            mainSequence = DOTween.Sequence().SetAutoKill(false);

            return mainSequence;
        }

        public override Tween GetShowTween(Action callBack = null)
        {
            mainSequence.Complete(true);

            mainSequence = DOTween.Sequence().SetAutoKill(false);

            return mainSequence;
        }
    }
}