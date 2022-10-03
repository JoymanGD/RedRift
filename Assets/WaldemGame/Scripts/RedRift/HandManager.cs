using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using External.MyBox;
using UnityEngine;
using UnityEngine.UI;
using WaldemGame;
using WaldemGame.Lorem;
using WaldemGame.Managers;

namespace RedRift
{
    public class HandManager : AManager
    {
#pragma warning disable 0649
    #region General
        [SerializeField]
        private Vector2 faceImageSize;
        [SerializeField]
        private Card cardPrefab;
        [SerializeField]
        private Transform cardsParent;
        [SerializeField]
        private RectTransform dropPanel;
        [SerializeField]
        private Button affectCardsButton;
        [SerializeField]
        private bool randomCardsAmount;
        [ConditionalField("randomCardsAmount")]
        [SerializeField]
        private int minCardsAmount = 4;
        [ConditionalField("randomCardsAmount")]
        [SerializeField]
        private int maxCardsAmount = 6;
        [ConditionalField("randomCardsAmount", true)]
        [SerializeField]
        private int cardsAmount = 5;
        [SerializeField]
        private HandShape deployBy;
    #endregion

    #region Curve
        [ConditionalField("deployBy", false, HandShape.Curve)]
        [SerializeField]
        private AnimationCurve handCurve;
        [ConditionalField("deployBy", false, HandShape.Curve)]
        [SerializeField]
        private float curveRotationMultiplier = 10;
    #endregion

    #region Arc
        [ConditionalField("deployBy", false, HandShape.Arc)]
        [SerializeField]
        private Vector2 generalOffset;
        [ConditionalField("deployBy", false, HandShape.Arc)]
        [SerializeField]
        private float cardsDistance = -180;
        [ConditionalField("deployBy", false, HandShape.Arc)]
        [SerializeField]
        private float arcHeight = -0.0008f;
        [ConditionalField("deployBy", false, HandShape.Arc)]
        [SerializeField]
        private float arcSymmetry = -.05f;
        [ConditionalField("deployBy", false, HandShape.Arc)]
        [SerializeField]
        private float arcRotationMultiplier = 10f;
    #endregion
#pragma warning restore 0649

        private List<Card> cards = new List<Card>();
        private bool isDeployed;
        private Sequence reduceSequence;

        public override void Init()
        {
            if(!Inited)
            {
                if(affectCardsButton)
                {
                    affectCardsButton.onClick.RemoveListener(AffectCards);
                    affectCardsButton.onClick.AddListener(AffectCards);
                }

                DeployCards();
                Inited = true;
            }
        }

        [ButtonMethod]
        public void ClearHand()
        {
            //TODO: change to object pooling
            foreach (var item in cards)
            {
                Destroy(item.gameObject);
            }

            cards.Clear();

            isDeployed = false;
        }

        [ButtonMethod]
        public async void DeployCards()
        {
            if(isDeployed)
            {
                ClearHand();
            }

            int currentCardsAmount = cardsAmount;

            if(randomCardsAmount)
            {
                currentCardsAmount = Random.Range(minCardsAmount, maxCardsAmount + 1);
            }

            var loremManager = App.GetManager<LoremManager>();
            var sprites = await loremManager.GetRandomImages(currentCardsAmount, faceImageSize.x, faceImageSize.y);

            for (int i = 0; i < currentCardsAmount; i++)
            {
                var sprite = sprites[i];

                var newCard = Instantiate(cardPrefab, cardsParent);
                newCard.SetFace(sprite);
                cards.Add(newCard);
            }

            isDeployed = true;

            PositionCards();
        }

        [ButtonMethod]
        public void PositionCards()
        {
            switch (deployBy)
            {
                case HandShape.Arc:
                    for (int i = 0; i < cards.Count; i++)
                    {
                        var currentCard = cards[i];
                        currentCard.Init();

                        float mid = cards.Count/2f;
                        var xOffset = -mid + i;
                        float x = xOffset * (cardsDistance + currentCard.RectTransform.sizeDelta.x);
                        float y = arcHeight*Mathf.Pow(x, 2) + arcSymmetry * x;

                        var cardPosition = new Vector2(x, y) + generalOffset;
                        
                        currentCard.RectTransform.localPosition = cardPosition;
                        currentCard.RectTransform.rotation = Quaternion.Euler(0,0, xOffset * -arcRotationMultiplier);
                        currentCard.CacheData();
                    }

                    break;
                case HandShape.Curve:
                    var curveStart = handCurve.keys[0].time;
                    var curveEnd = handCurve.keys[handCurve.length-1].time;

                    float realInterval = 1/(float)cards.Count;

                    for (int i = 0; i < cards.Count; i++)
                    {
                        float x = i * realInterval;
                        float normalizedX = Normalize(x, 0, 1, curveStart, curveEnd);
                        float y = handCurve.Evaluate(normalizedX);

                        Vector2 cardPosition = new Vector2(normalizedX, y);

                        var currentCard = cards[i];
                        currentCard.Init();
                        currentCard.RectTransform.localPosition = cardPosition;
                        currentCard.RectTransform.rotation = Quaternion.Euler(0,0, normalizedX * -curveRotationMultiplier);
                        currentCard.CacheData();
                    }

                    break;
            }
        }

        public void CheckDrop(Card card)
        {
            var mousePos = CameraManager.ScreenToWorldPoint(Input.mousePosition);

            if(dropPanel.rect.Contains(mousePos))
            {
                card.RectTransform.position = dropPanel.position;
                card.RectTransform.SetParent(dropPanel);
                
                DisableCard(card);

                PositionCards();
            }
            else
            {
                card.ReturnToHand();
            }
        }

        private void AffectCards()
        {
            if(reduceSequence.IsActive() && reduceSequence.IsPlaying())
            {
                reduceSequence.Complete(true);
                reduceSequence.Kill(true);
            }


            reduceSequence = DOTween.Sequence();

            foreach (var item in cards)
            {
                var cardChangeTween = item.GetChangeTween();

                reduceSequence.Append(cardChangeTween);

                item.SetInteractable(false);
            }

            reduceSequence.AppendCallback(()=> SetInteractable(true));

            reduceSequence.Play();
        }

        private void DisableCard(Card card)
        {
            cards.Remove(card);
            card.enabled = false;
        }

        private void SetInteractable(bool value)
        {
            foreach (var item in cards)
            {
                item.SetInteractable(value);
            }
        }

        private float Normalize(float val, float valMin, float valMax, float finalMin, float finalMax) 
        {
            return (((val - valMin) / (valMax - valMin)) * (finalMax - finalMin)) + finalMin;
        }

        private enum HandShape
        {
            Arc = 0,
            Curve = 1
        }
    }
}