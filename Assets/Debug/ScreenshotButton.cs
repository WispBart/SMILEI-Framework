using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenshotButton : MonoBehaviour
{

    public KeyCode Key = KeyCode.Alpha0;
    public string FileName = "Screenshot";

    public string path
    {
        #if UNITY_EDITOR
        get => Directory.GetCurrentDirectory() + "/" + FileName;
        #else
        get => Application.persistentDataPath + Path.PathSeparator + FileName;
        #endif
    }
    void Update()
    {
        if (Input.GetKeyDown(Key))
        {
            ScreenCapture.CaptureScreenshot(path);
            Debug.Log($"Screenshot saved to \"{path}\"");
        }
    }
}
