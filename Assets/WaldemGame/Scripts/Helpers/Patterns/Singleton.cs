using UnityEngine;

namespace WaldemGame.Helpers.Patterns
{
    public class Singleton<T> : MonoBehaviour where T : Object
    {
        public static T Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = FindObjectOfType<T>();
                }

                return instance;
            }
        }
        private static T instance;
    }
}