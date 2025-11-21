using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

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

    [Header("Ammo Mode: Reload")]
    [SerializeField] ReloadType reloadType;
    [SerializeField] int magSize;
    [SerializeField] float reloadSpeed;

    [Header("Fire Mode: Burst")]
    [Range(1, 16)][SerializeField] int numberOfBurstShots;

    [Header("Fire Mode: Manual")]
    [SerializeField] float boltActionTime;

    [Header("Spread")]
    [SerializeField] bool hasSpread;
    [SerializeField] float baseSpread;   
    [SerializeField] float spreadFireIncrease;
    [SerializeField] float spreadFireDecrease;
    [SerializeField] float spreadAirborneIncrease;
    [SerializeField] float spreadAirborneDecrease;
    [SerializeField] float spreadMultiplier;
    [SerializeField] float maxSpread;   

    [Header("Camera Shake")]
    [SerializeField] float strenght;
    [SerializeField] float initialSpeed;
    [SerializeField] float smoothTime;

    [Header("Visuals")]
    [SerializeField] GameObject impactEffect;
    [SerializeField] GameObject bulletTracer;
    [SerializeField] GameObject muzzleFlash;

    [Header("Decals")]
    [SerializeField] GameObject bulletHoleDecal;
    [SerializeField] GameObject bloodSplatterDecal;

    [Header("References")]
    [SerializeField] Transform muzzle;
    [SerializeField] LayerMask hitLayer;
        
    int currentAmmo; int ammoInMag; 
    float spread; float targetSpread; float currentFireSpread; float currentAirborneSpread; float fireCooldown;

    FirstPersonCamera fpsCam; AudioSource audioSource; PlayerHUD hud; WeaponHandler weaponHandler;

    [HideInInspector] public bool shooting; [HideInInspector] public bool reloading;

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
        Reload
    }

    public enum PointerType
    {
        Point,
        Crosshair
    }

    public enum ReloadType
    {
        Magazine,
        SingleLoading
    }

    public void PlayerTriggerPush()
    {
        StopCoroutine(SingleReload());

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

                case "Reload":
                    return ammoInMag;
            }

            return 0;
        }
    }

    IEnumerator Shoot()
    {
        shooting = true;

        reloading = false;
        StopCoroutine(MagReload());
        StopCoroutine(SingleReload());        
        GameManager.Instance.playerHUD.reloadText.SetActive(false);

        float burstShots = GetBurstFireShots();

        StartCoroutine(ShootLogic());
        
        fireCooldown = rateOfFire;

        yield return new WaitForSeconds(fireCooldown);

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
                fpsCam.StartShake(strenght, initialSpeed, smoothTime);
                GameManager.Instance.audioManager.PlayAudioPitch(audioSource, Random.Range(0.8f, 1.2f));
                Instantiate(muzzleFlash, muzzle.transform.position, Quaternion.identity, muzzle);

                ConsumeAmmo();
                StartCoroutine(weaponHandler.DisplayAmmo());

                yield return new WaitForSeconds(0.05f);
            }
        }

        void ShootBullet()
        {
            for (int i = 0; i < numberOfProyectiles; i++)
            {
                Vector3 shotOrigin = MousePositionSpreadOffset(); Quaternion cameraRotation = Camera.main.transform.rotation;
                Ray ray = Camera.main.ScreenPointToRay(shotOrigin); RaycastHit hit;

                ShootRaycast();
                SpawnBulletTracer();

                void ShootRaycast()
                {
                    if (Physics.Raycast(ray, out hit, range, hitLayer, QueryTriggerInteraction.Ignore))
                    {
                        CharacterNPC character = hit.transform.GetComponent<CharacterNPC>();

                        if (character != null)
                        {
                            character.TakeDamage(damage);
                            Instantiate(bloodSplatterDecal, hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
                        }
                        else
                        {
                            if (hit.transform.gameObject.layer == 7)
                            {
                                hit.transform.gameObject.SendMessage("TakeDamage", damage);
                            }

                            Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                            Instantiate(bulletHoleDecal, hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
                        }
                    }
                }

                void SpawnBulletTracer()
                {
                    if (bulletTracer != null)
                    {
                        //GameObject tracer = Instantiate(bulletTracer, Camera.main.ScreenToWorldPoint(shotOrigin), cameraRotation);
                        GameObject tracer = Instantiate(bulletTracer, muzzle.position, cameraRotation);
                        Vector3 hitPoint = ray.origin + (ray.direction.normalized * range);
                        tracer.GetComponent<BulletTracer>().SetDestination(hitPoint);
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

                case "Reload":
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

    public string GetAmmoString()
    {
        switch (ammoMode.ToString())
        {
            case "Total":
                return currentAmmo.ToString();

            case "Reload":
                return ammoInMag.ToString() + " / " + currentAmmo.ToString();
        }

        return null;
    }

    public void PlayerReload()
    {
        if (ammoMode.ToString() == "Reload")
        {
            if (ammoInMag != magSize && currentAmmo > 0 && !shooting && !reloading)
            {
                switch (reloadType.ToString())
                {
                    case "Magazine":
                        StartCoroutine(MagReload());
                        break;

                    case "SingleLoading":
                        StartCoroutine(SingleReload());
                        break;
                }
            }

            StartCoroutine(weaponHandler.DisplayAmmo());
        }                
    }

    IEnumerator MagReload()
    {
        reloading = true;

        GameManager.Instance.playerHUD.reloadText.SetActive(true);

        yield return new WaitForSeconds(reloadSpeed);

        currentAmmo += ammoInMag;
        ammoInMag = 0;

        int ammo = currentAmmo - magSize;
        ammo = Mathf.Clamp(ammo, 0, magSize);

        int ammoToMag = currentAmmo - ammo;
        ammoToMag = Mathf.Clamp(ammoToMag, 0, magSize);

        ammoInMag = ammoToMag;
        currentAmmo -= ammoToMag;

        reloading = false;

        StartCoroutine(weaponHandler.DisplayAmmo());

        GameManager.Instance.playerHUD.reloadText.SetActive(false);
    }

    IEnumerator SingleReload()
    {
        reloading = true;

        GameManager.Instance.playerHUD.reloadText.SetActive(true);
       
        int bulletsToReload = magSize - ammoInMag;

        for (int i = 0; i < bulletsToReload; i++)
        {
            if (currentAmmo > 0)
            {
                ammoInMag++;
                currentAmmo--;
                StartCoroutine(weaponHandler.DisplayAmmo());
                yield return new WaitForSeconds(reloadSpeed);
            }           
        }

        reloading = false;

        GameManager.Instance.playerHUD.reloadText.SetActive(false);
    }

    void ScriptReload()
    {
        if (ammoMode.ToString() == "Reload")
        {
            currentAmmo += ammoInMag;
            ammoInMag = 0;

            int ammo = currentAmmo - magSize;
            ammo = Mathf.Clamp(ammo, 0, magSize);

            int ammoToMag = currentAmmo - ammo;
            ammoToMag = Mathf.Clamp(ammoToMag, 0, magSize);

            ammoInMag = ammoToMag;
            currentAmmo -= ammoToMag;

            StartCoroutine(weaponHandler.DisplayAmmo());
        }      
    }

    void BulletSpreadControl()
    {
        if (hasSpread)
        {
            targetSpread = baseSpread + FireSpread() + AirborneSpread();
            spread = Mathf.Lerp(spread, targetSpread, spreadMultiplier);
            spread = Mathf.Clamp(spread, 0, maxSpread);
            
            GameManager.Instance.crosshairHandler.SetCrosshairSpread(spread);
        }

        //MousePositionDebug();

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
        return (Vector2) Input.mousePosition + Random.insideUnitCircle * spread;
    }

    /*
    void MousePositionDebug()
    {
        GameObject mouseTest = GameManager.Instance.debugManager.mousePosition;
        mouseTest.transform.position = MousePositionSpreadOffset();
    }
    */

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

    void SetCrosshairVisibility()
    {
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

    void GetReferences()
    {
        weaponHandler = transform.parent.GetComponent<WeaponHandler>();
        fpsCam = transform.parent.parent.GetComponent<FirstPersonCamera>();
        audioSource = GetComponent<AudioSource>();       
    }

    void Update()
    {
        BulletSpreadControl();
    }

    void Start()
    {
        currentAmmo = totalAmmo;
        ScriptReload();
    }

    void OnEnable()
    {
        GetReferences();      

        SetCrosshairVisibility();

        StartCoroutine(weaponHandler.DisplayAmmo());
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Ray ray = Camera.main.ScreenPointToRay(MousePositionSpreadOffset());
        Gizmos.DrawRay(ray);
    }
}
 