using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WaldemGame;
using WaldemGame.Abstract;
using WaldemGame.Managers;
using WaldemGame.Helpers.Extensions;
using System;

namespace RedRift
{
    public class Card : MonoBehaviour, IInitable, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
#pragma warning disable 0649
        [SerializeField]
        private Image face;
        [SerializeField]
        private Image shining;
        [SerializeField]
        private TextMeshProUGUI mana;
        [SerializeField]
        private TextMeshProUGUI attack;
        [SerializeField]
        private TextMeshProUGUI health;
        [SerializeField]
        private int minStartValue = 4;
        [SerializeField]
        private int maxStartValue = 12;
        [SerializeField]
        private TextMeshProUGUI notifier;
        [SerializeField]
        private float holdScaleMultiplier = 2;
        [SerializeField]
        private float selectScaleMultiplier = 1.3f;
        [SerializeField]
        private float returnToHandSpeed = .5f;
        [SerializeField]
        private float selectDuration = .3f;
        [SerializeField]
        private int minChangeValue = -2;
        [SerializeField]
        private int maxChangeValue = 9;
        [SerializeField]
        private float changeSpeed = .5f;
        [SerializeField]
        private float notifierDuration = .6f;
        [SerializeField]
        private Vector2 notifierStartOffset;
        [SerializeField]
        private Vector2 notifierMotion;
#pragma warning restore 0649

        public RectTransform RectTransform => rectTransform;
        public int Health { get => healthValue; set => SetValue(ValueType.Health, value); }
        public int Mana { get => manaValue; set => SetValue(ValueType.Mana, value); }
        public int Attack { get => attackValue; set => SetValue(ValueType.Attack, value); }
        public bool Inited { get; set; }

        private RectTransform rectTransform;
        private bool isSelected;
        private bool isHolding;
        private int selectedCardPreviousPlace;
        private Vector2 offset;
        private Vector3 cachedPosition;
        private Quaternion cashedRotation;
        private Vector3 cachedScale;
        private Sequence changeTween;
        [SerializeField]
        private int attackValue;
        [SerializeField]
        private int manaValue;
        [SerializeField]
        private int healthValue;
        [SerializeField]
        private int initialValue;
        [SerializeField]
        private int finalValue;
        private TextMeshProUGUI textToSync;
        private bool isInteractive = true;

        public void Init()
        {
            if(!Inited)
            {
                rectTransform = transform as RectTransform;

                var gameManager = App.GetManager<GameManager>();
                gameManager.OnGameUpdate.RemoveListener(MoveCard);
                gameManager.OnGameUpdate.AddListener(MoveCard);

                RandomizeValues();

                Inited = true;
            }
        }

        public void CacheData()
        {
            cachedPosition = rectTransform.position;
            cachedScale = rectTransform.localScale;
            cashedRotation = rectTransform.rotation;
        }

        public void RandomizeValues()
        {
            var valueTypesCount = Enum.GetValues(typeof(ValueType)).Length;

            for (int i = 0; i < valueTypesCount; i++)
            {
                var randomValue = UnityEngine.Random.Range(minStartValue, maxStartValue + 1);
                SetValue((ValueType)i, randomValue);
                SyncValueForced();
            }
        }

        public void SetInteractable(bool value)
        {
            isInteractive = value;
        }

        public void Reinit()
        {
            Inited = false;
            Init();
        }

        public void AsyncInit()
        {
        }

        public void SetFace(Sprite sprite)
        {
            face.sprite = sprite;
        }

        public void Select()
        {
            if(!isSelected)
            {
                SetFront();

                rectTransform.localScale *= selectScaleMultiplier;

                isSelected = true;
            }
        }

        public void Deselect()
        {
            if(isSelected)
            {
                SetBack();

                ResetScale();

                isSelected = false;
            }
        }

        public void SetValue(ValueType type, int newValue, bool add = false)
        {
            switch (type)
            {
                case ValueType.Attack:
                    initialValue = attackValue;
                    textToSync = attack;

                    finalValue = add ? initialValue + newValue : newValue;
                    attackValue = finalValue;
                    break;
                case ValueType.Mana:
                    initialValue = manaValue;
                    textToSync = mana;

                    finalValue = add ? initialValue + newValue : newValue;
                    manaValue = finalValue;
                    break;
                case ValueType.Health:
                    initialValue = healthValue;
                    textToSync = health;

                    finalValue = add ? initialValue + newValue : newValue;
                    healthValue = finalValue;
                    break;
            }
        }

        public Tween SyncValue()
        {
            var duration = Mathf.Abs((finalValue - initialValue)) / changeSpeed;
            return textToSync.DOTextInt(initialValue, finalValue, duration).SetEase(Ease.Linear);
        }

        public void SyncValueForced()
        {
            textToSync.text = finalValue.ToString();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(isInteractive)
            {
                Select();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(isInteractive)
            {
                Deselect();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(isInteractive)
            {
                isHolding = true;

                SetShine(true);

                rectTransform.DOKill();

                offset = rectTransform.position - CameraManager.ScreenToWorldPoint(Input.mousePosition);

                rectTransform.rotation = Quaternion.identity;

                rectTransform.localScale *= holdScaleMultiplier;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(isHolding)
            {
                var handManager = App.GetManager<HandManager>();
                handManager.CheckDrop(this);

                SetShine(false);

                ResetScale();

                offset = Vector2.zero;
                isHolding = false;
            }
        }

        public void ReturnToHand()
        {
            ResetRotation();
            ResetPosition(false);
        }

        public Tween GetChangeTween()
        {
            if(changeTween.IsActive() && changeTween.IsPlaying())
            {
                changeTween.Complete(true);
                changeTween.Kill(true);
            }

            var cameraManager = App.GetManager<CameraManager>();
            var cameraShakeTween = cameraManager.ShakeCamera(.4f, 30f);
            var valueType = GetRandomValueType();
            var changeValue = GetRandomChangeValue();
            SetValue(valueType, changeValue, true);
            var notifierTween = GetChangeNotifierTween(changeValue);

            changeTween = DOTween.Sequence();

                changeTween.Append(GetSelectTween());
                changeTween.Append(SyncValue());
                changeTween.Join(notifierTween);
                changeTween.Join(cameraShakeTween);

                changeTween.Append(GetDeselectTween());

            return changeTween;
        }

        private void SetShine(bool value)
        {
            shining.enabled = value;
        }

        private void SetFront()
        {
            selectedCardPreviousPlace = rectTransform.GetSiblingIndex();

            rectTransform.SetSiblingIndex(rectTransform.parent.childCount - 1);
        }

        private void SetBack()
        {
            if(selectedCardPreviousPlace >= 0)
            {
                rectTransform.SetSiblingIndex(selectedCardPreviousPlace);
                selectedCardPreviousPlace = -1;                    
            }
        }

        private Tween GetSelectTween()
        {
            Tween result = rectTransform.DOScale(cachedScale * selectScaleMultiplier, selectDuration);

            result.OnStart(()=>
            {
                SetFront();
                isSelected = true;
            });

            return result;
        }

        private Tween GetDeselectTween()
        {
            Tween result = rectTransform.DOScale(cachedScale, selectDuration);

            result.OnComplete(()=>
            {
                SetBack();
                isSelected = false;
            });

            return result;
        }

        private ValueType GetRandomValueType()
        {
            var randomIndex = UnityEngine.Random.Range(0,3);

            return (ValueType)randomIndex;
        }

        private int GetRandomChangeValue()
        {
            var changeValue = UnityEngine.Random.Range(minChangeValue, maxChangeValue + 1);

            return changeValue;
        }

        private Tween GetChangeNotifierTween(int value)
        {
            var notifierText = value.ToString();

            if(value >= 0)
            {
                notifierText = "+" + notifierText;
            }

            notifier.text = notifierText;

            Tween result = notifier.DONotifier(notifierDuration, (Vector2)textToSync.transform.position + notifierStartOffset, notifierMotion);

            return result;
        }

        private void MoveCard(float deltaTime)
        {
            if(isHolding)
            {
                rectTransform.position = CameraManager.ScreenToWorldPoint(Input.mousePosition) + (Vector3)offset;
            }
        }

        private void ResetRotation()
        {
            rectTransform.rotation = cashedRotation;
        }

        private void ResetScale()
        {
            rectTransform.localScale = cachedScale;
        }

        private void ResetPosition(bool forced = true)
        {
            if(forced)
            {
                rectTransform.position = cachedPosition;
            }
            else
            {
                var duration = Vector2.Distance(rectTransform.position, cachedPosition) / returnToHandSpeed;
                rectTransform.DOMove(cachedPosition, duration);
            }
        }
    }

    public enum ValueType
    {
        Attack = 0,
        Mana = 1,
        Health = 2
    }
}