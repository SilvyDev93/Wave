using System.IO;
using UnityEngine;

public class FullscreenOption : MonoBehaviour
{
    [SerializeField] OptionsEnableTextHandler optionsEnableHandler;

    public void SetWindowMode()
    {
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.FullScreenWindow:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                optionsEnableHandler.SetText("Off");
                Debug.Log("Set Windowed Mode");
                break;

            case FullScreenMode.Windowed:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                optionsEnableHandler.SetText("On");
                Debug.Log("Set Fullscreen Mode");
                break;
        }
    }

    public void CheckWindowMode()
    {
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.FullScreenWindow:
                optionsEnableHandler.SetText("On");
                break;

            case FullScreenMode.Windowed:
                optionsEnableHandler.SetText("Off");
                break;
        }
    }

    void OnEnable()
    {
        CheckWindowMode();   
    }
}
