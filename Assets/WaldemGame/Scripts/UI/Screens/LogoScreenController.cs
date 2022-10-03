using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace WaldemGame.UI.Screens
{
    public class LogoScreenController : MonoBehaviour
    {
    #pragma warning disable 0649
        [SerializeField]
        private Graphic logoImage;
        [SerializeField]
        private Graphic logoTitle;
        [SerializeField]
        private float appearDuration;
        [SerializeField]
        private float fadeInterval;
        [SerializeField]
        private float disappearDuration;
        [SerializeField]
        private float showDuration;
    #pragma warning restore 0649

        public Tween ShowTween()
        {
            Sequence mainSequence = DOTween.Sequence();

                Tween imageTween = logoImage.DOFade(1, appearDuration);
                Tween titleTween = logoTitle.DOFade(1, appearDuration);

            mainSequence
                .Append(imageTween)
                .AppendInterval(fadeInterval)
                .Append(titleTween);

            return mainSequence;
        }

        public Tween HideTween()
        {
            Sequence mainSequence = DOTween.Sequence();

                Tween imageTween = logoImage.DOFade(0, disappearDuration);
                Tween titleTween = logoTitle.DOFade(0, disappearDuration);

            mainSequence
                .Append(imageTween)
                .AppendInterval(fadeInterval)
                .Append(titleTween);

            return mainSequence;
        }

        public void PlayLogo(Action callBack = null)
        {
            Sequence mainSequence = DOTween.Sequence();

                Tween showTween = ShowTween();
                Tween hideTween = HideTween();

            mainSequence
                .Append(showTween)
                .AppendInterval(showDuration)
                .Append(hideTween)
                .AppendCallback(DisableUI)
                .AppendCallback(()=> callBack?.Invoke());

            mainSequence.Play();
        }

        private void DisableUI()
        {
            logoImage.gameObject.SetActive(false);
            logoTitle.gameObject.SetActive(false);
        }
    }
}