using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WindowPictureManager : MonoBehaviour
{
    public RectTransform SharePictureRectTransform;
    public RawImage ScreenshotImage;
    public ScanningAnimation ScanAnimation;
    public CameraManager CameraManager;
    public Button[] Buttons;
    public float AnimationTime;
    public AnimationCurve AnimationCurve;

    private const string FACE_MASK_SUBJECT = "Face Mask Subject";

    void Start()
    {
        SharePictureRectTransform.sizeDelta = new Vector2(1080, 1920);
        SharePictureRectTransform.anchoredPosition = new Vector2(0, -Screen.height);
        ActiveButtons(false);
    }

    public void OnEnable()
    {
        CameraManager.OnCameraSavedImage += ShowWindow;
    }

    public void OnDisable()
    {

    }

    public void Share()
    {
        new NativeShare().AddFile(CameraManager.CurrentPicturePath).SetSubject(FACE_MASK_SUBJECT).SetText("").Share();
    }

    public void ShowWindow()
    {
        StartCoroutine(MoveWindow(true, AnimationTime));
    }

    public void CloseWindow()
    {
        ActiveButtons(false);
        CameraManager.MainUITransform.gameObject.SetActive(true);
        ScanAnimation.ResetPositionAndSize();
        StartCoroutine(MoveWindow(false, AnimationTime));
    }

    private IEnumerator MoveWindow(bool up, float animationTime)
    {
        float elapsedTime = 0;
        while (elapsedTime < animationTime)
        {
            SharePictureRectTransform.anchoredPosition = new Vector2(0, Mathf.Lerp(up ? -Screen.height : 0, up ? 0 : -Screen.height, AnimationCurve.Evaluate(elapsedTime / animationTime)));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        SharePictureRectTransform.anchoredPosition = new Vector2(0, up ? 0 : -Screen.height);
        if (up)
        {
            ActiveButtons(true);
        }
    }

    private void ActiveButtons(bool active)
    {
        foreach (Button button in Buttons)
        {
            button.interactable = active;
        }
    }
}
