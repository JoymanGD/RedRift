using DG.Tweening;
using UnityEngine;

namespace WaldemGame.UI
{    
    public class APanelView : MonoBehaviour
    {
        [SerializeField]
        private PanelShowTypes showType;
        [SerializeField]
        private float showDuration = .4f;
        [SerializeField]
        private float hideDuration = .3f;

        private RectTransform rectTransform => transform as RectTransform;

        public virtual Tween ShowPanel(float delay = 0)
        {
            Vector3 tempPosition = transform.position;

            Vector3 hidePosition = tempPosition;

            switch (showType)
            {
                case PanelShowTypes.Up:
                    hidePosition.y = Screen.height + (Screen.height - transform.position.y) + rectTransform.sizeDelta.y / 2;
                    break;
                case PanelShowTypes.Down:
                    hidePosition.y = -transform.position.y - rectTransform.sizeDelta.y / 2;
                    break;
                case PanelShowTypes.Left:
                    hidePosition.x = -transform.position.x - rectTransform.sizeDelta.x / 2;
                    break;
                case PanelShowTypes.Right:
                    hidePosition.x = Screen.width + (Screen.width - transform.position.x) + rectTransform.sizeDelta.x / 2;
                    break;
            }

            transform.position = hidePosition;

            gameObject.SetActive(true);

            var tween = transform.DOMove(tempPosition, showDuration);
            tween.SetDelay(delay);

            return tween;
        }

        public virtual Tween HidePanel(float delay = 0)
        {
            Vector3 tempPosition = transform.position;

            Vector3 hidePosition = tempPosition;

            switch (showType)
            {
                case PanelShowTypes.Up:
                    hidePosition.y = Screen.height + (Screen.height - transform.position.y) + rectTransform.sizeDelta.y/2;
                    break;
                case PanelShowTypes.Down:
                    hidePosition.y = -transform.position.y - rectTransform.sizeDelta.y/2;
                    break;
                case PanelShowTypes.Left:
                    hidePosition.x = -transform.position.x - rectTransform.sizeDelta.x/2;
                    break;
                case PanelShowTypes.Right:
                    hidePosition.x = Screen.width + (Screen.width - transform.position.x) + rectTransform.sizeDelta.x/2;
                    break;
            }

            var tween = transform.DOMove(hidePosition, hideDuration);
            tween.OnComplete(()=>
            {
                gameObject.SetActive(false);

                transform.position = tempPosition;
            });

            tween.SetDelay(delay);
            
            return tween;
        }

        public enum PanelShowTypes
        {
            Up, Down, Left, Right
        }
    }
}