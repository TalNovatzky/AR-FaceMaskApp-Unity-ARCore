using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(Button))]
public class ToggleAnimation : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Button _button;

    public AnimationCurve AnimationCurve;
    public float AnimationTime = 0.5f;
    public float AnimationDegrees;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _button = GetComponent<Button>();
    }

    private void OnDisable()
    {
        _rectTransform.eulerAngles = new Vector3(0,0,0);
        StopAllCoroutines();
        _button.interactable = true;
    }

    public void TurnButton()
    {
        _button.interactable = false;
        StartCoroutine(AnimateTurn(AnimationDegrees, AnimationTime));
    }

    public IEnumerator AnimateTurn(float degrees, float time)
    {
        float currentDegrees = _rectTransform.eulerAngles.z;
        float targetDegrees = currentDegrees + degrees;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            _rectTransform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(currentDegrees, targetDegrees, AnimationCurve.Evaluate(elapsedTime / time)));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _button.interactable = true;
        _rectTransform.eulerAngles = new Vector3(0,0, targetDegrees);
    }

    void Update()
    {
        
    }
}
