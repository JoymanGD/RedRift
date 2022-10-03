using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WaldemGame.Inventory
{
    public class InventoryPanelElementView : MonoBehaviour
    {
        [SerializeField]
        private Image image;
        [SerializeField]
        private GameObject frameImage;
        [SerializeField]
        private TextMeshProUGUI amountText;

        public void SetElementView(Sprite sprite, int amount)
        {
            image.sprite = sprite;
            amountText.text = amount.ToString();
        }

        public void SetElementView(Tuple<Sprite, int> data)
        {
            SetElementView(data.Item1, data.Item2);
        }

        public void Enable(bool value)
        {
            image.gameObject.SetActive(value);
            amountText.gameObject.SetActive(value);
        }

        public void Select(bool value)
        {
            frameImage.SetActive(value);
        }
    }
}