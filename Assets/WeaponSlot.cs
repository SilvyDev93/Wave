using TMPro;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ammoCounter;
    [SerializeField] Vector2 unselectedSize;
    Vector2 originalSize;
    bool selected = true;

    public void Selection()
    {
        switch (selected)
        {
            case true:
                transform.localScale = unselectedSize;
                selected = false;
                break;

            case false:
                transform.localScale = originalSize;
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
        originalSize = transform.localScale;

        Selection();
    }
}
