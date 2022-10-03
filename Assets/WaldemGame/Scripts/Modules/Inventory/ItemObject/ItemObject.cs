using System;
using UnityEngine;

namespace WaldemGame.Inventory
{
    public class ItemObject : UsableObject
    {
        [SerializeField]
        private int id;
        [SerializeField]
        protected int count;

        public int Id => id;
        public int Count => count;

        public static ItemObject operator +(ItemObject a, ItemObject b)
        {
            if(a.Id == b.Id)
            {
                a.count += b.count;

                Destroy(b.gameObject);

                return a;
            }

            return null;
        }

        public static ItemObject operator -(ItemObject a, ItemObject b)
        {
            if(a.Id == b.Id && a.count > b.count)
            {
                a.count -= b.count;

                Destroy(b.gameObject);

                return a;
            }

            return null;
        }

        public static ItemObject operator +(ItemObject a, int count)
        {
            a.count += count;

            return a;
        }

        public static ItemObject operator -(ItemObject a, int count)
        {
            if(a.count > count)
            {
                a.count -= count;

                return a;
            }

            return null;
        }

        public ItemObject Clone()
        {
            return (ItemObject)this.MemberwiseClone();
        }
    }
}