using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WaldemGame.Inventory
{
    [CreateAssetMenu(menuName = "Data/Inventory/ItemsCollection", fileName = "NewItemsCollection")]
    public class ItemsCollectionSO : ScriptableObject
    {
        [SerializeField]
        private List<ItemData> items;

        public ItemData GetItem(int id)
        {
            return items.FirstOrDefault(i=> i.Id == id);
        }

        public ItemData GetItem(string name)
        {
            return items.FirstOrDefault(i=> i.Name == name);
        }

        public Sprite GetItemSprite(int id)
        {
            var item = GetItem(id);

            return item.Sprite;
        }
    }
}