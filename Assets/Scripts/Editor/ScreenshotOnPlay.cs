using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class ScreenshotOnPlay
{
    static string SCREENSHOT_PATH = "Assets/Screenshots/Screenshot_";
    static bool isPlaying = false;
    static float delayTime = 2f;

    static ScreenshotOnPlay()
    {
        EditorApplication.playmodeStateChanged += OnPlaymodeStateChange;
    }

    ~ScreenshotOnPlay()
    {
        EditorApplication.playmodeStateChanged -= OnPlaymodeStateChange;
    }

    static void OnPlaymodeStateChange()
    {
        #if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                isPlaying = !isPlaying;

                if (isPlaying)
                {
                    string path = "";
                    string time = System.DateTime.Now.ToString("yyyyMMddHHmmssffff");

                    path = SCREENSHOT_PATH + time + ".png";
                    // Instantiate a new gameobject that will capture a screenshot then delete itself.
                    // This works around a lack of coroutines in the editor.
                    GameObject go = new GameObject();
                    go.AddComponent<ScreenshotOnDelay>().SetTimer(delayTime, path);
                }
            }
        #endif
    }
}