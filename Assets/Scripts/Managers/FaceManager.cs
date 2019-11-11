using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class FaceManager : MonoBehaviour
{
    public Face[] Faces;
    public int CurrentIndex = 0;
    public ScanningAnimation ScanningAnimation;
    private int _maxIndex;
    private bool m_IsQuitting;
    private List<AugmentedFace> _augmentedFaces = new List<AugmentedFace>();

    private void Start()
    {
        _maxIndex = Faces.Length - 1;
        SwitchFace();
    }

    private void Update()
    {
        _UpdateApplicationLifecycle();
        Session.GetTrackables<AugmentedFace>(_augmentedFaces, TrackableQueryFilter.All);
        Screen.sleepTimeout = (_augmentedFaces.Count != 0 ? 15 : SleepTimeout.NeverSleep);
        Faces[CurrentIndex].gameObject.SetActive(_augmentedFaces.Count != 0);
        ScanningAnimation.MoveIcon(_augmentedFaces.Count != 0);
    }

    public void SwitchFace()
    {
        CurrentIndex += 1;

        if (CurrentIndex > _maxIndex)
        {
            CurrentIndex = 0;
        }

        ChangeFace();
    }

    private void ChangeFace()
    {
        for (int i = 0; i < Faces.Length; i++)
        {
           Faces[i].gameObject.SetActive(CurrentIndex == i);
            if (i == CurrentIndex)
            {
                ScanningAnimation.SetIcon(Faces[i].FaceIcon);
            }
        }
    }

    private void _UpdateApplicationLifecycle()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (m_IsQuitting)
        {
            return;
        }

        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            _ShowAndroidToastMessage("Camera permission is needed to run this application.");
            m_IsQuitting = true;
            Invoke("_DoQuit", 0.5f);
        }
        else if (Session.Status.IsError())
        {
            _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
            m_IsQuitting = true;
            Invoke("_DoQuit", 0.5f);
        }
    }

    private void _DoQuit()
    {
        Application.Quit();
    }

    private void _ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText",
                    unityActivity,
                    message,
                    0);
                toastObject.Call("show");
            }));
        }
    }
}
