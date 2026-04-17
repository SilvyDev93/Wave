using TMPro;
using UnityEngine;

public class OptionsEnableTextHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI enabledTMP;

    public void TextHandling()
    {
        switch (enabledTMP.text)
        {
            case "On":
                enabledTMP.text = "Off";
                break;

            case "Off":
                enabledTMP.text = "On";
                break;
        }
    }

    public void SetText(string text)
    {
        enabledTMP.text = text;
    }
}
