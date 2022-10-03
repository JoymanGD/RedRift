using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using System.Globalization;
using UnityEngine;

namespace WaldemGame.Helpers.Extensions
{
    public static class DoTweenExtension
    {
        public static Tweener DOTextInt(this TextMeshProUGUI text, int initialValue, int finalValue, float duration, Func<int, string> convertor)
        {
            return DOTween.To(() => initialValue, it => text.text = convertor(it), finalValue, duration);
        }

        public static Tweener DOTextInt(this TextMeshProUGUI text, int initialValue, int finalValue, float duration)
        {
            return DoTweenExtension.DOTextInt(text, initialValue, finalValue, duration, it => it.ToString());
        }

        public static Tweener DOTextFloat(this TextMeshProUGUI text, float initialValue, float finalValue, float duration, Func<float, string> convertor)
        {
            return DOTween.To(() => initialValue, it => text.text = convertor(it), finalValue, duration);
        }

        public static Tweener DOTextFloat(this TextMeshProUGUI text, float initialValue, float finalValue, float duration)
        {
            return DoTweenExtension.DOTextFloat(text, initialValue, finalValue, duration, it => it.ToString());
        }

        public static Tweener DOTextLong(this TextMeshProUGUI text, long initialValue, long finalValue, float duration, Func<long, string> convertor)
        {
            return DOTween.To(() => initialValue, it => text.text = convertor(it), finalValue, duration);
        }

        public static Tweener DOTextLong(this TextMeshProUGUI text, long initialValue, long finalValue, float duration)
        {
            return DoTweenExtension.DOTextLong(text, initialValue, finalValue, duration, it => it.ToString("N0", CultureInfo.InvariantCulture));
        }

        public static Tweener DOTextDouble(this TextMeshProUGUI text, double initialValue, double finalValue, float duration, Func<double, string> convertor)
        {
            return DOTween.To(() => initialValue, it => text.text = convertor(it), finalValue, duration);
        }

        public static Tweener DOTextDouble(this TextMeshProUGUI text, double initialValue, double finalValue, float duration)
        {
            return DoTweenExtension.DOTextDouble(text, initialValue, finalValue, duration, it => it.ToString());
        }

        public static Tween DONotifier(this Graphic graphic, float duration, Vector2 startPos, Vector2 motion, float fadeinStart = 0f, float motionStart = 0.2f, float fadeOutStart = 0.7f)
        {
            graphic.transform.position = startPos;
            
            var notifierSequence = DOTween.Sequence();

                var fadeInTween = graphic.DOFade(1, duration * motionStart);
                var motionTween = graphic.transform.DOMove(startPos+motion, duration * (1-motionStart));
                var fadeOutTween = graphic.DOFade(0, duration * (1-fadeOutStart));

                notifierSequence
                    .Insert(fadeinStart, fadeInTween)
                    .Insert(motionStart, motionTween)
                    .Insert(fadeOutStart, fadeOutTween);

            return notifierSequence;
        }

        public static Tween DOAppearWithPunch(this Transform appearingTransform, float appearDuration, float punchForce, float punchDuration, float punchElasticity, float startScale = 0f, float endScale = -1f, int vibrato = 10)
        {
            Vector3 tempScale;

            if (endScale < 0)
            {
                tempScale = appearingTransform.localScale;
            }
            else
            {
                tempScale = Vector3.one * endScale;
            }
            
            var appearSequence = DOTween.Sequence();

            Tween scale = appearingTransform.DOScale(tempScale, appearDuration).SetEase(Ease.Linear).From(startScale);

            Vector3 punchVector = Vector3.one * punchForce;
            Tween punch = appearingTransform.DOPunchScale(punchVector, punchDuration, vibrato , elasticity: punchElasticity).SetEase(Ease.Linear);

            appearSequence.
                Append(scale).
                Append(punch);

            appearSequence.OnStart(()=>
            {
                appearingTransform.localScale = Vector3.one * startScale;
                appearingTransform.gameObject.SetActive(true);
            });

            return appearSequence;
        }

        public static Tween DOAppearWithPunch(this Transform appearingTransform, float appearDuration, float amplitude = 1.1f, float period = 0.333f)
        {
            return appearingTransform.DOScale(1, appearDuration).SetEase(Ease.OutElastic, amplitude, period).From(0);
        }

        public static Tween DOAppearWithPunch(this Transform appearingTransform, float appearDuration, AnimationCurve curve)
        {
            return appearingTransform.DOScale(1, appearDuration).SetEase(curve).From(0);
        }

        public static Tween DODisappearWithPunch(this Transform appearingTransform, float appearDuration, float amplitude = 1.1f, float period = 0.333f)
        {
            return appearingTransform.DOScale(Vector3.zero, appearDuration).SetEase(Ease.InElastic, amplitude, period).From(appearingTransform.localScale);
        }

        public static Tween DOAppearByFade(this Graphic appearingGraphic, float fadeDuration)
        {
            Color tempColor = appearingGraphic.color;
            float tempOpacity = tempColor.a;

            tempColor.a = 0;
            appearingGraphic.color = tempColor;

            appearingGraphic.gameObject.SetActive(true);

            var appearTween = appearingGraphic.DOFade(tempOpacity, fadeDuration);

            return appearTween;
        }

        public static Tween DOAppearByScale(this Transform objectTransform, float duration, Action callBack = null)
        {
            var scaleTween = objectTransform.DOScale(Vector3.one, duration).From(Vector3.zero);

            scaleTween.OnComplete(()=> 
            {
                callBack?.Invoke();
            });

            return scaleTween;
        }

        public static Tween DODisappearByScale(this Transform objectTransform, float duration, Action callBack = null)
        {
            var scaleTween =  objectTransform.DOScale(Vector3.zero, duration).From(objectTransform.localScale);
            
            scaleTween.OnComplete(()=> 
            {
                callBack?.Invoke();
            });

            return scaleTween;
        }

        public static Tween GetReversedTween(this Tween innerTween) //TODO: test
        {
            Sequence reversedTween = DOTween.Sequence();

            reversedTween.Append(innerTween);

            reversedTween.Complete(false);
            reversedTween.isBackwards = true;

            return reversedTween;
        }

        /// <summary>
        /// Hides rect out of screen. On tween's completion deactivates rect's gameObject and resets the position.
        /// </summary>
        /// <param name="animationSide">The side to hide out of</param>
        /// <param name="duration">Duration of hiding</param>
        /// <param name="postCallBack">Executes on tween's completion</param>
        /// <returns></returns>
        public static Tween DOHide(this RectTransform elementRectTransform, TransformAnimationSides animationSide, float duration, Action preCallBack = null, Action postCallBack = null)
        {
            if(DOTween.IsTweening(elementRectTransform))
            {
                elementRectTransform.DOComplete(true);
            }

            preCallBack?.Invoke();

            var anchoredPosition = elementRectTransform.anchoredPosition;
            
            Vector3 hideValue = GetHideValue(elementRectTransform, animationSide);

            Tween hideTween = elementRectTransform.DOAnchorPos(hideValue, duration).From(anchoredPosition).OnComplete(()=> 
            {
                elementRectTransform.gameObject.SetActive(false);

                elementRectTransform.anchoredPosition = anchoredPosition;

                postCallBack?.Invoke();
            });

            return hideTween;
        }

        /// <summary>
        /// Shows rect in the screen. The rect must be already at show position before tweening, as the tweener acts in such order: hides rect -> sets active -> tweens to show.
        /// </summary>
        /// <param name="animationSide">The side to show in from</param>
        /// <param name="duration">Duration of showing</param>
        /// <param name="postCallBack">Executes on tween's completion</param>
        /// <returns>Tween</returns>
        public static Tween DOShow(this RectTransform elementRectTransform, TransformAnimationSides animationSide, float duration, Action preCallBack = null, Action postCallBack = null)
        {
            if(DOTween.IsTweening(elementRectTransform))
            {
                elementRectTransform.DOComplete(true);
            }
            
            preCallBack?.Invoke();
            
            Vector3 anchoredPosition = elementRectTransform.anchoredPosition;
            Vector3 hideValue = GetHideValue(elementRectTransform, animationSide);

            elementRectTransform.anchoredPosition = hideValue;

            elementRectTransform.gameObject.SetActive(true);

            Tween showTween = elementRectTransform.DOAnchorPos(anchoredPosition, duration)
                //.From(hideValue)
                .OnComplete(()=> postCallBack?.Invoke());

            return showTween;
        }

        /// <summary>
        /// Sets hide tween's direction to backwards. Also sets hide tween's target to the inner RectTransform so it can be reusable.
        /// </summary>
        /// <param name="UIElementHideAnimation">Hide tween</param>
        /// <returns>Tween</returns>
        public static Tween DOUnhide(this RectTransform elementRectTransform, Tween UIElementHideAnimation)
        {
            if(DOTween.IsTweening(elementRectTransform))
            {
                elementRectTransform.DOComplete();
            }
            
            if(UIElementHideAnimation.IsComplete() == false)
            {
                UIElementHideAnimation.Complete();
            }

            UIElementHideAnimation.target = elementRectTransform;

            UIElementHideAnimation.isBackwards = true;

            return UIElementHideAnimation;
        }

        private static Vector3 GetHideValue(RectTransform elementRectTransform, TransformAnimationSides animationSide)
        {
            //var position = elementRectTransform.anchoredPosition;
            var height = elementRectTransform.sizeDelta.y;
            var width = elementRectTransform.sizeDelta.x;
            //var screenWidth = Screen.width;
            //var screenHeight = Screen.height;


            Vector3 hideValue = elementRectTransform.anchoredPosition;

            switch (animationSide)
            {
                case TransformAnimationSides.Up:
                    hideValue.y = -hideValue.y;
                    hideValue.y += height;
                    break;
                case TransformAnimationSides.Down:
                    hideValue.y = -hideValue.y;
                    hideValue.y += -height;
                    break;
                case TransformAnimationSides.Right:
                    hideValue.x = -hideValue.x;
                    hideValue.x += width;
                    break;
                case TransformAnimationSides.Left:
                    hideValue.x = -hideValue.x;
                    hideValue.x += -width;
                    break;
            }

            return hideValue;
        }

        public static Tween DOPunchColor(this Graphic targetImage, Color color, float duration)
        {
            targetImage.DOComplete(true);

            var tempColor = targetImage.color;

            Sequence punchSequence = DOTween.Sequence();

                Tween forwardTween = targetImage.DOColor(color, duration/2);
                Tween backwardTween = targetImage.DOColor(tempColor, duration/2);

                punchSequence
                    .Append(forwardTween)
                    .Append(backwardTween);

            return punchSequence;
        }

        public static Tween DOPunchFade(this Graphic targetImage, float alpha, float duration)
        {
            var targetColor = targetImage.color;
            var finalColor = new Color(targetColor.r, targetColor.g, targetColor.b, alpha);
            return targetImage.DOPunchColor(finalColor, duration);
        }

        public enum TransformAnimationSides
        {
            Up = 0,
            Down = 1,
            Left = 2,
            Right = 3
        }
    }
}