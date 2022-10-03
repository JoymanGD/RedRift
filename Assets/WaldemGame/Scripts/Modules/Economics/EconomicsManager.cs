using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using WaldemGame.Managers;

namespace WaldemGame.Economics
{
    public class EconomicsManager : AManager
    {
        [SerializeField]
        private ResourcesPanelView resourcesPanel;
        [SerializeField]
        private List<ResourceData> playerResources;
        [SerializeField]
        private ResourceDatasSO resourcesVisualData;
        [SerializeField]
        private float initPanelDelay = 2;

        public override void Init()
        {
            resourcesPanel.SyncResourcesView(playerResources);
            resourcesPanel.ShowPanel(initPanelDelay);
        }

        public void AddResource(ResourceType type, int amount)
        {
            var existingResource = playerResources.FirstOrDefault(r=> r.Type == type);
            
            if(existingResource == null)
            {
                var newResource = new ResourceData(type, amount);
                playerResources.Add(newResource);
            }
            else
            {
                existingResource.AddAmount(amount);
            }

            resourcesPanel.SyncResourcesView(playerResources);
        }

        public Sprite GetResourceIcon(ResourceType type)
        {
            return resourcesVisualData.GetIcon(type);
        }
    }

    [Serializable]
    public class ResourceData
    {
        [SerializeField]
        private ResourceType type;
        [SerializeField]
        private int amount;

        public ResourceType Type => type;
        public int Amount => amount;

        public ResourceData(ResourceType type, int amount)
        {
            this.type = type;
            this.amount = amount;
        }

        public void AddAmount(int amount)
        {
            this.amount += amount;
        }
    }

    public enum ResourceType
    {
        Money = 0
    }
}