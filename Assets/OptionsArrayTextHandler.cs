using TMPro;
using UnityEngine;

public class OptionsArrayTextHandler : MonoBehaviour
{
    [SerializeField] string[] optionsStringArray;
    [SerializeField] int[] optionsTextSize;
    [SerializeField] TextMeshProUGUI arrayTMP;

    int index = 0;

    public void TextHandling()
    {
        index += 1;

        if (index > optionsStringArray.Length - 1) 
        {
            index = 0;
        }

        arrayTMP.text = optionsStringArray[index];
        arrayTMP.fontSize = optionsTextSize[index];
    }

    void Awake()
    {
        arrayTMP.text = optionsStringArray[0];
        arrayTMP.fontSize = optionsTextSize[0];
    }
}
