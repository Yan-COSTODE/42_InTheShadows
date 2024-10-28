using System;
using System.Collections;
using UnityEngine;

public class UIIrisWipe : MonoBehaviour
{
    public event Action OnOutFinished;
    public event Action OnInFinished;
    
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private GameObject panel;
    [SerializeField] private EEasing easingIn = EEasing.EASE_OUT_QUAD;
    [SerializeField] private EEasing easingOut = EEasing.EASE_IN_QUAD;
    [SerializeField] private Vector2 finalScale;
    [SerializeField] private Vector2 minimalScale;

    public void OnDestroy()
    {
        OnOutFinished = null;
        OnInFinished = null;
    }

    public void FadeIn(float _time)
    {
        StartCoroutine(Tween(minimalScale, finalScale, _time, easingIn, true));
    }

    public void FadeOut(float _time)
    {
        StartCoroutine(Tween(finalScale, minimalScale, _time, easingOut, false));
    }
    
    private IEnumerator Tween(Vector2 _start, Vector2 _end, float _time, EEasing _easing, bool _in)
    {
        if (!rectTransform)
            yield break;
        
        float _startTime = Time.unscaledTime;
        panel.SetActive(true);

        while (Time.unscaledTime - _startTime < _time)
        {
            float _ease = Easing.Ease((Time.unscaledTime - _startTime) / _time, _easing);
            rectTransform.sizeDelta = Vector2.Lerp(_start, _end, _ease);
            yield return null;
        }
        
        rectTransform.sizeDelta = _end;
        panel.SetActive(false);

        if (_in)
        {
            OnInFinished?.Invoke();
            OnInFinished = null;
        }
        else
        {
            OnOutFinished?.Invoke();
            OnOutFinished = null;
        }
    }
}