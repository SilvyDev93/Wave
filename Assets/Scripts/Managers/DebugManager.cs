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

    void Awake()
    {
        StartCoroutine(FrameRateDisplay());
    }
}
