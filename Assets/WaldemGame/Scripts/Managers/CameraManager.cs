using DG.Tweening;
using UnityEngine;
using WaldemGame.Abstract;

namespace WaldemGame.Managers
{    
    public class CameraManager : AManager, ILateUpdatable
    {
    #pragma warning disable 0649
        [SerializeField]
        private Camera mainCamera;
        [SerializeField]
        private Camera loadingCamera;
        [SerializeField]
        private bool followTarget = true;
        [SerializeField]
        private Transform target;
        [SerializeField]
        private float followSpeed;
    #pragma warning restore 0649

        private Tween cameraShakeTween;

    public static Camera MainCamera;

        public override void Init()
        {
            if(!Inited)
            {
                mainCamera.gameObject.SetActive(true);
                
                var gameManager = App.GetManager<GameManager>();

                gameManager.OnGameLateUpdate.RemoveListener(DoLateUpdate);
                gameManager.OnGameLateUpdate.AddListener(DoLateUpdate);

                MainCamera = mainCamera;

                Inited = true;
            }
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        public Tween ShakeCamera(float duration, float strength = 3, int vibrato = 10, float randomness = 90, bool fadeOut = true)
        {
            if(cameraShakeTween.IsActive())
            {
                cameraShakeTween.Complete(true);
                cameraShakeTween.Kill(true);
            }

            cameraShakeTween = mainCamera.DOShakePosition(duration, strength, vibrato, randomness, fadeOut);

            return cameraShakeTween;
        }

        public static Vector3 ScreenToWorldPoint(Vector3 screenPosition)
        {
            return MainCamera.ScreenToWorldPoint(screenPosition);
        }

        public void DoLateUpdate(float deltaTime)
        {
            if(followTarget && target)
            {
                var newPos = mainCamera.transform.position;
                var cacheZ = newPos.z;
                newPos = Vector3.Lerp(newPos, target.position, Time.deltaTime * followSpeed);
                newPos.z = cacheZ;
                mainCamera.transform.position = newPos;
            }
        }
    }
}