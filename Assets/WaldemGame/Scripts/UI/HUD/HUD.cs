using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using WaldemGame.Abstract;

namespace WaldemGame.UI.HUD
{
    public abstract class HUD : MonoBehaviour, IInitable
    {
        public abstract Tween GetHideTween(Action callBack = null);
        public abstract Tween GetShowTween(Action callBack = null);

        public bool Inited { get; set; } = false;

        public void Init()
        {
            if(!Inited)
            {
                InternalInit();
                Inited = true;
            }
        }

        protected abstract void InternalInit();

        public void Reinit()
        {
            Inited = false;
            Init();
        }

        public void AsyncInit()
        {
        }
    }
}