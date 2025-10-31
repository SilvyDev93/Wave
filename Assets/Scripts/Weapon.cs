using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Configuration")]
    [SerializeField] GameObject proyectile;
    public AmmoMode ammoMode;
    public FireMode fireMode;
    public PointerType pointerType;

    [Header("Weapon Parameters")]
    [SerializeField] float damage;
    [SerializeField] float rateOfFire;
    [SerializeField] int totalAmmo;   
    [SerializeField] int range;
    [SerializeField] float recoil;
    [SerializeField] [Range(1, 16)] int numberOfProyectiles;
    [SerializeField] [Range(1, 16)] int bulletPenetration;

    [Header("Spread")]
    [SerializeField] bool hasSpread;
    [SerializeField] float baseSpread;   
    [SerializeField] float spreadFireIncrease;
    [SerializeField] float spreadFireDecrease;
    [SerializeField] float spreadAirborneIncrease;
    [SerializeField] float spreadAirborneDecrease;
    [SerializeField] float spreadMultiplier;
    [SerializeField] float maxSpread;

    [Header("Ammo Mode: Mag")]
    [SerializeField] int magSize;

    [Header("Fire Mode: Burst")]
    [Range(1, 16)] [SerializeField] int numberOfBurstShots;

    [Header("Fire Mode: Manual")]
    [SerializeField] float boltActionTime;

    [Header("Camera Shake")]
    [SerializeField] float strenght;
    [SerializeField] float initialSpeed;
    [SerializeField] float smoothTime;

    public enum FireMode
    {
        Automatic,
        Semiautomatic,
        Manual,
        Burst
    }

    public enum AmmoMode
    {
        Total,
        Mag
    }
    
    public enum PointerType
    {
        Point,
        Crosshair
    }

    [Header("References")]    
    [SerializeField] CameraShake cameraShake;

    int currentAmmo; int ammoInMag; float fireCooldown;
    float spread; float targetSpread; float currentFireSpread; float currentAirborneSpread;

    [HideInInspector] public bool shooting;

    FirstPersonCamera fpsCam; AudioSource audioSource; PlayerHUD hud;

    public void PlayerTriggerPush()
    {
        if (!shooting && GetCurrentAmmoMode() > 0)
        {
            StartCoroutine(Shoot());
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
    }

    IEnumerator Shoot()
    {
        shooting = true;

        yield return new WaitForSeconds(fireCooldown);

        float burstShots = GetBurstFireShots();

        StartCoroutine(ShootLogic());
        
        fireCooldown = rateOfFire;

        StartCoroutine(ManualBoltAction());

        IEnumerator ShootLogic()
        {
            for (int i = 0; i < burstShots; i++)
            {
                if (proyectile == null)
                {
                    ShootBullet();
                }
                else
                {
                    ShootProyectile();
                }

                fpsCam.FireRecoil(recoil);
                cameraShake.StartShake(strenght, initialSpeed, smoothTime);
                GameManager.Instance.audioManager.PlayAudioPitch(audioSource, Random.Range(0.8f, 1.2f));

                ConsumeAmmo();
                DisplayToHUD();

                yield return new WaitForSeconds(0.05f);
            }
        }

        void ShootBullet()
        {
            for (int i = 0; i < numberOfProyectiles; i++)
            {
                Ray ray = Camera.main.ScreenPointToRay(MousePositionSpreadOffset());
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, range))
                {
                    CharacterNPC character = hit.transform.GetComponent<CharacterNPC>();

                    if (character != null)
                    {
                        character.TakeDamage(damage);
                    }
                }
            }
        }

        void ShootProyectile()
        {
            for (int i = 0; i < numberOfProyectiles; i++)
            {
                Instantiate(proyectile, Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.rotation);               
            }
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

        IEnumerator ManualBoltAction()
        {
            switch (fireMode.ToString())
            {
                case "Manual":
                    yield return new WaitForSeconds(boltActionTime);
                    shooting = false;
                    break;

                default:
                    shooting = false;
                    break;
            }
        }       
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
        if (ammoMode.ToString() == "Mag")
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
        }

        DisplayToHUD();
    }

    void BulletSpreadControl()
    {
        targetSpread = baseSpread + FireSpread() + AirborneSpread();
        spread = Mathf.Lerp(spread, targetSpread, spreadMultiplier);
        spread = Mathf.Clamp(spread, 0, maxSpread);

        MousePositionDebug();
        GameManager.Instance.crosshairHandler.SetCrosshairSpread(spread);

        float FireSpread()
        {
            if (shooting)
            {
                currentFireSpread += spreadFireIncrease;
                currentFireSpread = Mathf.Clamp(currentFireSpread, 0, maxSpread);
                return currentFireSpread;
            }
            else
            {
                currentFireSpread -= spreadFireDecrease;
                currentFireSpread = Mathf.Clamp(currentFireSpread, 0, maxSpread);
                return currentFireSpread;
            }
        }

        float AirborneSpread()
        {
            if (!GameManager.Instance.playerController.OnGround())
            {
                currentAirborneSpread += spreadAirborneIncrease;
                currentAirborneSpread = Mathf.Clamp(currentAirborneSpread, 0, maxSpread);
                return currentAirborneSpread;
            }
            else
            {
                currentAirborneSpread -= spreadAirborneDecrease;
                currentAirborneSpread = Mathf.Clamp(currentAirborneSpread, 0, maxSpread);
                return currentAirborneSpread;
            }
        }
    }

    Vector3 MousePositionSpreadOffset()
    {
        return Input.mousePosition + new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), 0);
    }

    void MousePositionDebug()
    {
        GameObject mouseTest = GameManager.Instance.debugManager.mousePosition;
        mouseTest.transform.position = MousePositionSpreadOffset();
    }

    int GetBurstFireShots()
    {
        if (fireMode.ToString() == "Burst")
        {
            return numberOfBurstShots;
        }
        else
        {
            return 1;
        }
    }

    void Update()
    {
        BulletSpreadControl();
    }

    void Start()
    {
        currentAmmo = totalAmmo;
        ScriptReload();
        DisplayToHUD();
    }

    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        hud = GameManager.Instance.playerHUD;
        fpsCam = transform.parent.parent.GetComponent<FirstPersonCamera>();        
        ScriptReload();
        DisplayToHUD();

        switch (pointerType.ToString())
        {
            case "Crosshair":
                GameManager.Instance.crosshairHandler.SetCrosshairActive(true);
                break;

            default:
                GameManager.Instance.crosshairHandler.SetCrosshairActive(false);
                break;
        }
    }
}
 