using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(Image))]
public class ScanningAnimation : MonoBehaviour
{
    private const string ANIM_IS_TRACKING_BOOL = "IsTracking";
    private const string MOVE_ICON_COROUTINE = "MoveAndResizeIcon";
    private RectTransform _rectTransform;
    private bool _firstTime = true;
    private bool _isTracking = false;

    public Image Icon;
    public Vector2 MaximazedSize;
    public RectTransform MaximazedRectTarget;
    public Vector2 MinimazedSize;
    public RectTransform MinimazedRectTarget;
    public float AnimationTime = 1f;
    public AnimationCurve AnimationCurve;
    public Animator Animator;

    public void SetIcon(Sprite sprite)
    {
        Icon.sprite = sprite;
    }

    private void OnEnable()
    {
        Animator.SetBool(ANIM_IS_TRACKING_BOOL, _isTracking);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void MoveIcon(bool isTracking)
    {
        if(isTracking == _isTracking && !_firstTime) return;

        if (_firstTime) _firstTime = false;

        _isTracking = isTracking;
        Animator.SetBool(ANIM_IS_TRACKING_BOOL, isTracking);
        StopCoroutine(MOVE_ICON_COROUTINE);
        StartCoroutine(MoveAndResizeIcon(AnimationTime));
    }

    public void ResetPositionAndSize()
    {
        _rectTransform.anchoredPosition =
            _isTracking ? MinimazedRectTarget.anchoredPosition : MaximazedRectTarget.anchoredPosition;
        _rectTransform.sizeDelta = _isTracking ? MinimazedSize : MaximazedSize;
    }

    public IEnumerator MoveAndResizeIcon(float time)
    {
        Vector2 currentSizeDelta = _rectTransform.sizeDelta;
        Vector2 currentPosition = _rectTransform.anchoredPosition;
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            _rectTransform.sizeDelta = Vector2.Lerp(currentSizeDelta, _isTracking ? MinimazedSize : MaximazedSize,
                AnimationCurve.Evaluate(elapsedTime / time));

            _rectTransform.anchoredPosition = Vector2.Lerp(currentPosition, _isTracking ? MinimazedRectTarget.anchoredPosition : MaximazedRectTarget.anchoredPosition, AnimationCurve.Evaluate(elapsedTime / time));

            elapsedTime += Time.deltaTime;
            yield return  new WaitForEndOfFrame();
        }
    }

    void Start()
    {
        Icon = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
    }
}
