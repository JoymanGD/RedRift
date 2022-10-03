using WaldemGame.Inventory;
using UnityEngine;

namespace WaldemGame.Quests
{
    [CreateAssetMenu(menuName = "Data/Quests/CollectQuest", fileName = "NewCollectQuest")]
    public class CollectQuest : AQuest
    {
        [SerializeField]
        private int collectObjectID;
        [SerializeField]
        private int amountToCollect;

        public override bool IsCompleted()
        {
            var inventoryManager = App.GetManager<InventoryManager>();
            var collectedAmount = inventoryManager.GetItemAmount(collectObjectID);

            return collectedAmount >= amountToCollect;
        }

        public override void QuestCompletedAction()
        {
            var inventoryManager = App.GetManager<InventoryManager>();
            inventoryManager.ThrowItemAway(collectObjectID, amountToCollect);

            base.QuestCompletedAction();
        }
    }
}