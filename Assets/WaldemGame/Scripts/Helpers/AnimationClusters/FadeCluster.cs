using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace WaldemGame.Helpers.AnimationClusters
{
    public class FadeCluster : MonoBehaviour, IAnimationCluster
    {
    #pragma warning disable 0649
        [SerializeField]
        private bool WithDisabling; //TODO: add conditional field to cluster parent
        [SerializeField]
        private GameObject ClusterParent;
        [SerializeField]
        private List<Graphic> FadableElements;
        [SerializeField]
        private float FadeInValue = 1;
        [SerializeField]
        private float FadeInDuration = .35f;
        [SerializeField]
        private float FadeOutValue = 0;
        [SerializeField]
        private float FadeOutDuration = .35f;
    #pragma warning restore 0649

        public void AnimateForward()
        {
            GetForwardAnimation().Play();
        }

        public void AnimateBackward()
        {
            GetBackwardInanimation().Play();
        }

        public Tween GetForwardAnimation()
        {
            Sequence animation = DOTween.Sequence();

            foreach (Graphic item in FadableElements)
            {
                animation.Join(item.DOFade(FadeInValue, FadeInDuration).From(FadeOutValue));
            }

            if(WithDisabling)
            {
                animation.OnStart(()=>
                {
                    ClusterParent.SetActive(true);
                });
            }

            return animation;
        }

        public Tween GetBackwardInanimation()
        {
            Sequence inanimation = DOTween.Sequence();

            foreach (Graphic item in FadableElements)
            {
                inanimation.Join(item.DOFade(FadeOutValue, FadeOutDuration).From(FadeInValue));
            }

            if(WithDisabling)
            {
                inanimation.OnComplete(()=>
                {
                    ClusterParent.SetActive(false);
                });
            }

            return inanimation;
        }
    }
}