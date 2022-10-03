namespace WaldemGame.Inventory
{
    public class ConfirmableItemObject : ItemObject
    {
        protected override void UsableAction()
        {
            var inventoryManager = App.GetManager<InventoryManager>();
            inventoryManager.PickUpItem(this);
            StopUsing();
        }
    }
}