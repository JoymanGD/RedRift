using System.Threading.Tasks;
using UnityEngine;
using WaldemGame.Abstract;

namespace WaldemGame.Managers
{
    public abstract class AManager : MonoBehaviour, IInitable
    {
        public bool Inited { get; set;}

        public virtual void Init()
        {
            
        }

    #pragma warning disable 1998
        public virtual async void AsyncInit()
        {
        }
    #pragma warning restore 1998


        public void Reinit()
        {
            Inited = false;
            Init();
        }
    }
}