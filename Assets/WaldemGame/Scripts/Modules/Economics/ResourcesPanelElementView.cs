using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WaldemGame.Economics
{
    public class ResourcesPanelElementView : MonoBehaviour
    {
        [SerializeField]
        private Image resourceIcon;
        [SerializeField]
        private TextMeshProUGUI resourceAmount;

        public void Set(Sprite sprite, int amount)
        {
            resourceIcon.sprite = sprite;
            resourceAmount.text = amount.ToString();
        }

        public virtual void Enable(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}