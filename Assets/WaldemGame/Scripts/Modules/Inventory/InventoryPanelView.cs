using System;
using System.Collections.Generic;
using WaldemGame.UI;
using UnityEngine;

namespace WaldemGame.Inventory
{
    public class InventoryPanelView : APanelView
    {
        [SerializeField]
        private InventoryPanelElementView elementViewPrefab;

        private List<InventoryPanelElementView> elements;

        public void SetupPanelView(int elementsAmount)
        {
            elements = new List<InventoryPanelElementView>();

            for (int i = 0; i < elementsAmount; i++)
            {
                var newElement = Instantiate(elementViewPrefab, transform);
                newElement.Enable(false);

                elements.Add(newElement);
            }
        }

        public void ClearPanel()
        {
            foreach (var item in elements)
            {
                item.Enable(false);
            }
        }

        public void SetElements(List<Tuple<Sprite, int>> itemObjects)
        {
            for (int i = 0; i < itemObjects.Count; i++)
            {
                var elementView = elements[i];
                var itemData = itemObjects[i];

                elementView.SetElementView(itemData);

                elementView.Enable(true);
            }
        }

        public void SelectElement(int index)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                var elementView = elements[index: i];
                elementView.Select(i == index);
            }
        }
    }
}