using External.MyBox;
using UnityEngine;

namespace WaldemGame.Inventory
{
    public class CollectableResourceItem : ConfirmableItemObject
    {
        [SerializeField]
        private int toolID = -1;
        [SerializeField]
        private int health = 1;
        [SerializeField]
        private bool randomCollectAmount;
        [SerializeField]
        [ConditionalField("randomCollectAmount")]
        private int randomRangeSize;

        protected override void UsableAction()
        {
            var inventoryManager = App.GetManager<InventoryManager>();

            if(inventoryManager.CurrentItemObject && inventoryManager.CurrentItemObject.Id == toolID)
            {
                health--;

                if(health <= 0)
                {
                    var actualCollectAmount = Count;

                    if(randomCollectAmount)
                    {
                        var min = Mathf.Clamp(actualCollectAmount - randomRangeSize, 0, actualCollectAmount);
                        var max = actualCollectAmount + randomRangeSize;

                        actualCollectAmount = Random.Range(min, max + 1);
                    }
                    
                    inventoryManager.PickUpItem(this, actualCollectAmount);

                    inventoryManager.UseTool();

                    StopUsing();
                }
            }
        }

        //TODO: show tip only when right tool is in the hand
        // protected override void OnTriggerEnterAction(Collider2D other)
        // {
        //     var inventoryManager = App.GetManager<InventoryManager>();

        //     if(inventoryManager.CurrentItemObject && inventoryManager.CurrentItemObject.Id == toolID)
        //     {
        //         StartUsing();
        //     }
        // }
    }    
}
