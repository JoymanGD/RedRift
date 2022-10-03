using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using WaldemGame.Managers;
using System;
using Waldem.Unity.Events;
using DG.Tweening;

namespace WaldemGame.Inventory
{
    public class InventoryManager : AManager
    {
        [SerializeField]
        private ItemsCollectionSO itemsCollection;
        [SerializeField]
        private int maxItemsAmount = 9;
        [SerializeField]
        private InventoryPanelView panel;
        [SerializeField]
        private float initPanelDelay = 2;

        private List<ItemObject> playerItems;
        private int currentItemIndex;

        [HideInInspector]
        public IntEvent OnItemSelected = new IntEvent();
        public ItemObject CurrentItemObject => GetCurrentItemObject();
        public ClassicEvent OnToolUsed = new ClassicEvent();

        public override void Init()
        {
            if(!Inited)
            {
                playerItems = new List<ItemObject>();
                currentItemIndex = -1;

                panel.SetupPanelView(maxItemsAmount);

                var inputManager = App.GetManager<InputManager>();

                inputManager.OnMouseWheelScrolled.RemoveListener(MouseScrolledHandler);
                inputManager.OnMouseWheelScrolled.AddListener(MouseScrolledHandler);

                panel.ShowPanel(initPanelDelay);
            }
        }

        public void PickUpItem(ItemObject item, int amount = 1)
        {
            if(playerItems.Count < maxItemsAmount)
            {
                var existingObject = playerItems.FirstOrDefault(i=> i.Id == item.Id);

                if(existingObject == null)
                {
                    playerItems.Add(item.Clone());
                }
                else
                {
                    existingObject += amount;
                }

                Destroy(item.gameObject);

                if(playerItems.Count == 1)
                {
                    SelectObject(0);
                }

                UpdateView();
            }
        }

        public ItemObject GetCurrentItemObject()
        {
            ItemObject result = null;

            if(currentItemIndex > -1)
            {
                result = playerItems[currentItemIndex];
            }

            return result;
        }

        private void MouseScrolledHandler(float scrollDelta)
        {
            var intDelta = (int)scrollDelta;

            if(intDelta > 0)
            {
                PreviousItem();
            }
            else
            {
                NextItem();
            }
        }

        public void NextItem()
        {
            var nextItemIndex = currentItemIndex + 1;
            nextItemIndex = nextItemIndex % maxItemsAmount;

            if(playerItems.Count > nextItemIndex)
            {
                SelectObject(nextItemIndex);
            }
        }

        public void PreviousItem()
        {
            var nextItemIndex = currentItemIndex <= 0 ? maxItemsAmount - 1 : currentItemIndex - 1;

            if(playerItems.Count > nextItemIndex)
            {
                SelectObject(nextItemIndex);
            }
        }

        public int GetItemAmount(int id)
        {
            int result = 0;

            var item = playerItems.FirstOrDefault(i=> i.Id == id);

            if(item)
            {
                result = item.Count;
            }

            return result;
        }

        public void ThrowItemAway(int id, int count = 1000)
        {
            if(playerItems.Count > 0)
            {
                var existingObject = playerItems.FirstOrDefault(i=> i.Id == id);

                if(existingObject == null || count <= 0)
                {
                    return;
                }

                if(count >= existingObject.Count)
                {
                    playerItems.Remove(existingObject);

                    //TODO: change to object pooling
                    Destroy(existingObject.gameObject);
                }
                else
                {
                    existingObject -= count;
                }

                UpdateView();
            }
        }

        public void UpdateView()
        {
            if(playerItems.Count > 0)
            {
                List<Tuple<Sprite, int>> itemsData = new List<Tuple<Sprite, int>>();

                foreach (var item in playerItems)
                {
                    var sprite = itemsCollection.GetItemSprite(item.Id);

                    itemsData.Add(new Tuple<Sprite, int>(sprite, item.Count));
                }

                panel.ClearPanel();
                panel.SetElements(itemsData);
            }
        }

        public void UseTool()
        {
            OnToolUsed?.Invoke();
        }

        private void SelectObject(int index)
        {
            currentItemIndex = index;

            panel.SelectElement(currentItemIndex);
            OnItemSelected?.Invoke(currentItemIndex);
        }

        public Tween ShowInventoryPanel()
        {
            return panel.ShowPanel();
        }

        public Tween HideInventoryPanel()
        {
            return panel.HidePanel();
        }

        private void SaveInventoryData()
        {

        }

        private void LoadInventoryData()
        {

        }
    }
}