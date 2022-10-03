using WaldemGame.Economics;
using UnityEngine;

namespace WaldemGame.Reward
{
    [CreateAssetMenu(menuName = "Data/Quests/Rewards/MoneyReward", fileName = "NewMoneyReward")]
    public class MoneyReward : AReward
    {
        [SerializeField]
        private int moneyAmount;

        public override void ReleaseReward()
        {
            var economicsManager = App.GetManager<EconomicsManager>();
            economicsManager.AddResource(ResourceType.Money, moneyAmount);
        }
    }
}