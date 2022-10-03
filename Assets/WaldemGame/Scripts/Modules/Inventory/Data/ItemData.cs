using System;
using UnityEngine;

namespace WaldemGame.Inventory
{    
    [Serializable]
    public struct ItemData
    {
        public string Name;
        public int Id;
        public Sprite Sprite;
    }
}