using System.Collections;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI framerateText;

    IEnumerator FrameRateDisplay()
    {
        framerateText.text = ((int)(1f / Time.unscaledDeltaTime)).ToString() + " fps";
        yield return new WaitForSeconds(1);
        StartCoroutine(FrameRateDisplay());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ScreenCapture.CaptureScreenshot("screenshot" + Random.Range(1, 99) + ".png", 0);
        }
    }

    void Awake()
    {
        StartCoroutine(FrameRateDisplay());
    }
}
