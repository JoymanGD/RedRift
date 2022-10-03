using UnityEngine;
using WaldemGame.Abstract;

namespace WaldemGame.Managers
{    
    public class MenuManager : AManager
    {
        public override void Init()
        {
            if(!Inited)
            {
                Inited = true;
            }
        }
    }
}