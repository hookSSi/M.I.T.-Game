using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace MIT.SamtleGame.Tools
{
    /// <summary>
    /// Events used to trigger faders on or off
    /// </summary>
    public struct FadeEvent
    {
        public int _id;
        public float _duration;
        public float _targetAlpha;
        public Tweens.TweenCurve _curve;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoreMountains.MMInterface.MMFadeEvent"/> struct.
        /// </summary>
        /// <param name="duration">Duration, in seconds.</param>
        /// <param name="targetAlpha">Target alpha, from 0 to 1.</param>
        public FadeEvent(float duration, float targetAlpha, Tweens.TweenCurve tween = Tweens.TweenCurve.LinearTween, int id=0)
        {
            _id = id;
            _duration = duration;
            _targetAlpha = targetAlpha;
            _curve = tween;

        }
        static FadeEvent e;
        public static void Trigger(float duration, float targetAlpha, Tweens.TweenCurve tween = Tweens.TweenCurve.LinearTween, int id = 0)
        {
            e._id = id;
            e._duration = duration;
            e._targetAlpha = targetAlpha;
            e._curve = tween;
            EventManager.TriggerEvent(e);
        }
    }
     
    public struct FadeInEvent
    {
        public int _id;
        public float _duration;
        public Tweens.TweenCurve _curve;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoreMountains.MMInterface.MMFadeInEvent"/> struct.
        /// </summary>
        /// <param name="duration">Duration.</param>
        public FadeInEvent(float duration, Tweens.TweenCurve tween = Tweens.TweenCurve.LinearTween, int id = 0)
        {
            _id = id;
            _duration = duration;
            _curve = tween;
        }
        static FadeInEvent e;
        public static void Trigger(float duration, Tweens.TweenCurve tween = Tweens.TweenCurve.LinearTween, int id = 0)
        {
            e._id = id;
            e._duration = duration;
            e._curve = tween;
            EventManager.TriggerEvent(e);
        }
    }

    public struct FadeOutEvent
    {
        public int _id;
        public float _duration;
        public Tweens.TweenCurve _curve;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoreMountains.MMInterface.MMFadeOutEvent"/> struct.
        /// </summary>
        /// <param name="duration">Duration.</param>
        public FadeOutEvent(float duration, Tweens.TweenCurve tween = Tweens.TweenCurve.LinearTween, int id = 0)
        {
            _id = id;
            _duration = duration;
            _curve = tween;
        }

        static FadeOutEvent e;
        public static void Trigger(float duration, Tweens.TweenCurve tween = Tweens.TweenCurve.LinearTween, int id = 0)
        {
            e._id = id;
            e._duration = duration;
            e._curve = tween;
            EventManager.TriggerEvent(e);
        }
    }

    /// <summary>
    /// The Fader class can be put on an Image, and it'll intercept MMFadeEvents and turn itself on or off accordingly.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Image))]
    public class Fader : MonoBehaviour, EventListener<FadeEvent>, EventListener<FadeInEvent>, EventListener<FadeOutEvent>
    {
        [Header("Identification")]
        /// the ID for this fader (0 is default), set more IDs if you need more than one fader
        public int _id;
        [Header("Opacity")]
        /// the opacity the fader should be at when inactive
        public float _inactiveAlpha = 0f;
        /// the opacity the fader should be at when active
        public float _activeAlpha = 1f;
        [Header("Timing")]
        /// the default duration of the fade in/out
        public float _defaultDuration = 0.2f;
        /// the default curve to use for this fader
        public Tweens.TweenCurve _defaultTween = Tweens.TweenCurve.LinearTween;
        /// whether or not the fade should happen in unscaled time 
        public bool _ignoreTimescale = true;
        [Header("Interaction")]
        /// whether or not the fader should block raycasts when visible
        public bool _shouldBlockRaycasts = false;
        
        [InspectorButton("FadeIn1Second")]
        public bool _fadeIn1SecondButton;
        [InspectorButton("FadeOut1Second")]
        public bool _fadeOut1SecondButton;
        [InspectorButton("DefaultFade")]
        public bool _defaultFadeButton;
        [InspectorButton("ResetFader")]
        public bool _resetFaderButton;

        protected CanvasGroup _canvasGroup;
        protected Image _image;

        protected float _initialAlpha;
        protected float _currentTargetAlpha;
        protected float _currentDuration;
        protected Tweens.TweenCurve _currentCurve;

        protected bool _fading = false;
        protected float _fadeStartedAt;

        /// <summary>
        /// Test method triggered by an inspector button
        /// </summary>
        protected virtual void ResetFader()
        {
            _canvasGroup.alpha = _inactiveAlpha;
        }

        /// <summary>
        /// Test method triggered by an inspector button
        /// </summary>
        protected virtual void DefaultFade()
        {
            FadeEvent.Trigger(_defaultDuration, _activeAlpha, _defaultTween, _id);
        }

        /// <summary>
        /// Test method triggered by an inspector button
        /// </summary>
        protected virtual void FadeIn1Second()
        {
            FadeInEvent.Trigger(1f);
        }

        /// <summary>
        /// Test method triggered by an inspector button
        /// </summary>
        protected virtual void FadeOut1Second()
        {
            FadeOutEvent.Trigger(1f);
        }

        /// <summary>
        /// On Start, we initialize our fader
        /// </summary>
        protected virtual void Start()
        {
            Initialization();
        }

        /// <summary>
        /// On init, we grab our components, and disable/hide everything
        /// </summary>
        protected virtual void Initialization()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = _inactiveAlpha;

            _image = GetComponent<Image>();
            _image.enabled = false;
        }

        /// <summary>
        /// On Update, we update our alpha 
        /// </summary>
        protected virtual void Update()
        {
            if (_canvasGroup == null) { return; }

            if (_fading)
            {
                Fade();
            }
        }

        /// <summary>
        /// Fades the canvasgroup towards its target alpha
        /// </summary>
        protected virtual void Fade()
        {
            float currentTime = _ignoreTimescale ? Time.unscaledTime : Time.time;
            float endTime = _fadeStartedAt + _currentDuration;
            if (currentTime - _fadeStartedAt < _currentDuration)
            {
                float result = Tweens.Tween(currentTime, _fadeStartedAt, endTime, _initialAlpha, _currentTargetAlpha, _currentCurve);
                _canvasGroup.alpha = result;
            }
            else
            {
                StopFading();
            }
        }

        /// <summary>
        /// Stops the fading.
        /// </summary>
        protected virtual void StopFading()
        {
            _canvasGroup.alpha = _currentTargetAlpha;
            _fading = false;
            if (_canvasGroup.alpha == _inactiveAlpha)
            {
                DisableFader();
            }
        }

        /// <summary>
        /// Disables the fader.
        /// </summary>
        protected virtual void DisableFader()
        {
            _image.enabled = false;
            if (_shouldBlockRaycasts)
            {
                _canvasGroup.blocksRaycasts = false;
            }
        }

        /// <summary>
        /// Enables the fader.
        /// </summary>
        protected virtual void EnableFader()
        {
            _image.enabled = true;
            if (_shouldBlockRaycasts)
            {
                _canvasGroup.blocksRaycasts = true;
            }
        }

        protected virtual void StartFading(float initialAlpha, float endAlpha, float duration, Tweens.TweenCurve curve, int id)
        {
            if (id != _id)
            {
                return;
            }
            EnableFader();
            _fading = true;
            _initialAlpha = initialAlpha;
            _currentTargetAlpha = endAlpha;
            _fadeStartedAt = _ignoreTimescale ? Time.unscaledTime : Time.time;
            _currentCurve = curve;
            _currentDuration = duration;
        }

        /// <summary>
        /// When catching a fade event, we fade our image in or out
        /// </summary>
        /// <param name="fadeEvent">Fade event.</param>
        public virtual void OnEvent(FadeEvent fadeEvent)
        {
            _currentTargetAlpha = (fadeEvent._targetAlpha == -1) ? _activeAlpha : fadeEvent._targetAlpha;
            StartFading(_canvasGroup.alpha, _currentTargetAlpha, fadeEvent._duration, fadeEvent._curve, fadeEvent._id);
        }

        /// <summary>
        /// When catching an MMFadeInEvent, we fade our image in
        /// </summary>
        /// <param name="fadeEvent">Fade event.</param>
        public virtual void OnEvent(FadeInEvent fadeEvent)
        {
            StartFading(_inactiveAlpha, _activeAlpha, fadeEvent._duration, fadeEvent._curve, fadeEvent._id);
        }

        /// <summary>
        /// When catching an MMFadeOutEvent, we fade our image out
        /// </summary>
        /// <param name="fadeEvent">Fade event.</param>
        public virtual void OnEvent(FadeOutEvent fadeEvent)
        {
            StartFading(_activeAlpha, _inactiveAlpha, fadeEvent._duration, fadeEvent._curve, fadeEvent._id);
        }

        /// <summary>
        /// On enable, we start listening to events
        /// </summary>
        protected virtual void OnEnable()
        {
            this.EventStartListening<FadeEvent>();
            this.EventStartListening<FadeInEvent>();
            this.EventStartListening<FadeOutEvent>();
        }

        /// <summary>
        /// On disable, we stop listening to events
        /// </summary>
        protected virtual void OnDisable()
        {
            this.EventStopListening<FadeEvent>();
            this.EventStopListening<FadeInEvent>();
            this.EventStopListening<FadeOutEvent>();
        }
    }
}