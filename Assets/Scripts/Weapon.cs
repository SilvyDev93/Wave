using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Configuration")]
    public AmmoMode ammoMode;
    public FireMode fireMode;

    [Header("Weapon Parameters")]
    [SerializeField] float damage;
    [SerializeField] float fireRate;
    [SerializeField] int totalAmmo;   
    [SerializeField] int range;
    [SerializeField] float recoil;
    [SerializeField] float spread;

    [Header("Ammo Mode: Mag")]
    [SerializeField] int magSize;

    public enum FireMode
    {
        Automatic,
        Semiautomatic,
        Manual,
    }

    public enum AmmoMode
    {
        Total,
        Mag
    }   

    [Header("References")]    
    [SerializeField] CameraShake cameraShake;

    int currentAmmo; int ammoInMag; float fireCooldown; bool shooting;

    AudioSource audioSource; PlayerHUD hud;

    public void PlayerTriggerPush()
    {
        if (!shooting && GetCurrentAmmoMode() > 0)
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        shooting = true;

        yield return new WaitForSeconds(fireCooldown);
        BulletSpread();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            CharacterNPC character = hit.transform.GetComponent<CharacterNPC>();

            if (character != null)
            {
                character.TakeDamage(damage);
            }
        }

        transform.parent.parent.GetComponent<FirstPersonCamera>().FireRecoil(recoil);
        cameraShake.StartShake(1, 1, 0.2f);
        GameManager.Instance.audioManager.PlayAudioPitch(audioSource, Random.Range(0.8f, 1.2f));

        ConsumeAmmo();
        DisplayToHUD();
        fireCooldown = fireRate;

        shooting = false;
    }

    void BulletSpread()
    {
        GameObject mouseTest = GameManager.Instance.debugManager.mousePosition;
        mouseTest.transform.position = Input.mousePosition + new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), 0);
        GameManager.Instance.crosshairHandler.SetCrosshairSpread(spread);
    }

    void DisplayToHUD()
    {
        switch (ammoMode.ToString())
        {
            case "Total":
                hud.ammoCounter.text = currentAmmo.ToString();
                break;

            case "Mag":
                hud.ammoCounter.text = ammoInMag.ToString() + " / " + currentAmmo.ToString();
                break;
        }
    }

    public void PlayerReload()
    {
        ScriptReload();
    }

    void ScriptReload()
    {
        if (ammoInMag != magSize && currentAmmo > 0)
        {
            currentAmmo += ammoInMag;
            ammoInMag = 0;

            int ammo = currentAmmo - magSize;
            ammo = Mathf.Clamp(ammo, 0, magSize);
            int ammoToMag = currentAmmo - ammo;

            ammoInMag = ammoToMag;
            currentAmmo -= ammoToMag;
        }

        DisplayToHUD();
    }


    void ConsumeAmmo()
    {
        switch (ammoMode.ToString())
        {
            case "Total":
                currentAmmo--;
                break;

            case "Mag":
                ammoInMag--;
                break;
        }
    }

    int GetCurrentAmmoMode()
    {
        switch (ammoMode.ToString())
        {
            case "Total":
                return currentAmmo;

            case "Mag":
                return ammoInMag;
        }

        return 0;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        hud = GameManager.Instance.playerHUD;
        currentAmmo = totalAmmo;
        ScriptReload();      
    }
}
 