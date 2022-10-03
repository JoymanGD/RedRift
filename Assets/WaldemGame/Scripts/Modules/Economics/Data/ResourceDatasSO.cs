using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WaldemGame.Economics
{
    [CreateAssetMenu(menuName = "Data/Economics/ResourceVisualData", fileName = "NewResourceVisualData")]
    public class ResourceDatasSO : ScriptableObject
    {
        public List<ResourceVisualData> resources;

        public Sprite GetIcon(ResourceType type)
        {
            Sprite icon = null;

            var existingResource = resources.FirstOrDefault(r=> r.Type == type);
            
            if(existingResource != null)
            {
                icon = existingResource.Icon;
            }

            return icon;
        }
    }

    [Serializable]
    public class ResourceVisualData
    {
        public ResourceType Type;
        public Sprite Icon;
    }
}