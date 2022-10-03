using DG.Tweening;

namespace WaldemGame.Helpers.AnimationClusters
{
    public interface IAnimationCluster
    {
        void AnimateForward();
        void AnimateBackward();
        Tween GetForwardAnimation();
        Tween GetBackwardInanimation();
    }
}
