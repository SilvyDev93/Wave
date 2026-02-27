using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] Vector2 selectedScale;
    [SerializeField] Vector2 reducedScale;

    [Header("References")]
    [SerializeField] TextMeshProUGUI ammoCounter;
    [SerializeField] RawImage weaponPortrait;

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

    public void SetWeaponPortrait(Texture2D portrait)
    {
        weaponPortrait.texture = portrait;
    }

    private void Start()
    {
        Selection();
    }
}
