namespace WaldemGame.Abstract
{
    internal interface IFixedUpdatable : IBaseUpdatable
    {
        void DoFixedUpdate(float deltaTime);
    }
}