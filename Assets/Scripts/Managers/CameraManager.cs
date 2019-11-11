using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Texture2D CurrentTexture;
    public string CurrentPicturePath;
    public Transform MainUITransform;
    public WindowPictureManager WindowPictureManager;

    public delegate void CameraSavedImage();
    public event CameraSavedImage OnCameraSavedImage;

    public void TakePicture()
    {
        StartCoroutine(SaveImage());
    }

    private IEnumerator SaveImage()
    {
        MainUITransform.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();

        CurrentTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        CurrentTexture.ReadPixels(new Rect(0,0,Screen.width, Screen.height),0,0);
        CurrentTexture.Apply();

        string path = Path.Combine(Application.temporaryCachePath, "shared image.png");
        File.WriteAllBytes(path, CurrentTexture.EncodeToPNG());

        WindowPictureManager.ScreenshotImage.texture = CurrentTexture;
        CurrentPicturePath = path;

        OnCameraSavedImage?.Invoke();
    }
}
