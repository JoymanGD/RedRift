using UnityEngine;

namespace WaldemGame.Inventory
{
    public class InstantlyPickableItemObject : ItemObject
    {
        protected override void OnTriggerEnterAction(Collider2D other)
        {
            if(other.tag == "Player")
            {
                var inventoryManager = App.GetManager<InventoryManager>();
                inventoryManager.PickUpItem(this);
            }
        }

        protected override void OnTriggerExitAction(Collider2D other)
        {
            
        }
    }
}