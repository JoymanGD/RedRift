using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using WaldemGame.Managers;
using WaldemGame.Abstract;
using WaldemGame.Helpers.Patterns;
using System.Threading.Tasks;

namespace WaldemGame
{
    public class App : Singleton<App>
    {
    #pragma warning disable 0649
        [SerializeField]
        private int frameRate = 144;
        #region MonoManagers
            [SerializeField]
            private List<AManager> managers;
            [SerializeField]
            private List<AManager> gameCoreManagers;
        #endregion
    #pragma warning restore 0649
        #region System
            private List<IInitable> initables = new List<IInitable>();
            private List<IUpdatable> updatables = new List<IUpdatable>();
            private List<IFixedUpdatable> fixedUpdatables = new List<IFixedUpdatable>();
            private List<ILateUpdatable> lateUpdatables = new List<ILateUpdatable>();
        #endregion

        private static List<AManager> allManagers = new List<AManager>();

        #region MonoMethods
            private void Awake()
            {
                SetupAplication();

                allManagers.AddRange(managers);
                allManagers.AddRange(gameCoreManagers);

                AssignInitables();
                AssignUpdatables();

                GetManager<LoadingManager>().Init();
                GetManager<GameManager>().Init();
            }

            private void SetupAplication()
            {
                Application.targetFrameRate = frameRate;
            }

            public void StartInitializing()
            {
                float progress = 0;
                float fraction = 1f/initables.Count;

                var loadingManager = GetManager<LoadingManager>();

                loadingManager.Sync(progress);

                foreach (var item in initables)
                {
                    item.Init();

                    progress += fraction;
                    loadingManager.Sync(progress);
                }
            }

            private void Update()
            {
                foreach (var iUpdatable in updatables)
                {
                    iUpdatable.DoUpdate(Time.deltaTime);
                }
            }

            private void FixedUpdate()
            {
                foreach (var iFixedUpdatable in fixedUpdatables)
                {
                    iFixedUpdatable.DoFixedUpdate(Time.deltaTime);
                }
            }

            private void LateUpdate()
            {
                foreach (var iLateUpdatable in lateUpdatables)
                {
                    iLateUpdatable.DoLateUpdate(Time.deltaTime);
                }
            }
        #endregion

        #region PublicMethods
            public static T GetManager<T>() where T : AManager
            {
                var type = typeof(T);
                return (T)allManagers.FirstOrDefault(m=> m.GetType() == type);
            }
        #endregion

        #region PrivateMethods

            private void AssignInitables()
            {
                foreach (var item in allManagers)
                {
                    if(item is IInitable iInitable)
                    {
                        initables.Add(iInitable);
                    }
                }
            }

            private void AssignUpdatables()
            {
                foreach (var item in allManagers)
                {
                    if(item is IUpdatable iUpdatable)
                    {
                        updatables.Add(iUpdatable);
                    }

                    if(item is IFixedUpdatable iFixedUpdatable)
                    {
                        fixedUpdatables.Add(iFixedUpdatable);
                    }

                    if(item is ILateUpdatable iLateUpdatable)
                    {
                        lateUpdatables.Add(iLateUpdatable);
                    }
                }
            }
        #endregion
    }
}