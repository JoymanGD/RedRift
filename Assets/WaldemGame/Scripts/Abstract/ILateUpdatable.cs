namespace WaldemGame.Abstract
{
    internal interface ILateUpdatable : IBaseUpdatable
    {
        void DoLateUpdate(float deltaTime);
    }
}