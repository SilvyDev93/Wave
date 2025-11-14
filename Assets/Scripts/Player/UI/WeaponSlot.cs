using TMPro;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] Vector2 selectedScale;
    [SerializeField] Vector2 reducedScale;

    [Header("References")]
    [SerializeField] TextMeshProUGUI ammoCounter;

    bool selected = true;

    public void Selection()
    {
        switch (selected)
        {
            case true:
                transform.localScale = reducedScale;
                selected = false;
                break;

            case false:
                transform.localScale = selectedScale;
                selected = true;
                break;
        }
    }

    public void SetAmmoString(string text)
    {
        ammoCounter.text = text;
    }

    private void Start()
    {
        Selection();
    }
}
