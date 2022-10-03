namespace WaldemGame.Abstract
{
    internal interface IUpdatable : IBaseUpdatable
    {
        void DoUpdate(float deltaTime);
    }
}