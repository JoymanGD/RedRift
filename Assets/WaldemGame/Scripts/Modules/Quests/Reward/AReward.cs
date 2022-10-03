using UnityEngine;

namespace WaldemGame.Reward
{
    public abstract class AReward : ScriptableObject
    {
        public abstract void ReleaseReward();
    }
}