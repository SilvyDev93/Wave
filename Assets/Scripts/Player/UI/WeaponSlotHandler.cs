using System.Collections;
using UnityEngine;

public class WeaponSlotHandler : MonoBehaviour
{
    [SerializeField] GameObject weaponSlotPrefab;
    [SerializeField] float yOffset;
    [SerializeField] float activationDelay;

    IEnumerator WeaponSlotsCreation()
    {
        yield return new WaitForSeconds(activationDelay);

        for (int i = 0; i < GameManager.Instance.weaponHandler.playerWeapons.Length; i++)
        {
            GameObject slot = Instantiate(weaponSlotPrefab, transform);
            slot.transform.position = new Vector2(slot.transform.position.x, slot.transform.position.y + (i * yOffset));
            slot.name = "Slot " + i;
        }

        GameManager.Instance.weaponHandler.slotsReady = true;
    }

    void Start()
    {
        StartCoroutine(WeaponSlotsCreation());
        StartCoroutine(GameManager.Instance.weaponHandler.DisplayAmmo());
    }
}
