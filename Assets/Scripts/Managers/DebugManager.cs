using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI framerateText;

    private void Update()
    {
        framerateText.text = ((int)(1f / Time.unscaledDeltaTime)).ToString() + " fps"; 
    }
}
