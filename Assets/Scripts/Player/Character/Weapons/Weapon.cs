using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.Image;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Information")]
    public string weaponName;
    public int price;
    public int weight;
    public int sellValue;

    [Header("Weapon Configuration")]
    [SerializeField] GameObject proyectile;
    public AmmoMode ammoMode;
    public FireMode fireMode;
    public PointerType pointerType;

    [Header("Weapon Parameters")]
    [SerializeField] DamageParameters damageParameters;
    [SerializeField] float rateOfFire;
    [SerializeField] int totalAmmo;   
    [SerializeField] int range;
    [SerializeField] float recoil;
    [SerializeField] [Range(1, 16)] int numberOfProyectiles;
    [SerializeField] [Range(1, 16)] int bulletPenetration;
    [SerializeField] float knockback;
    [SerializeField] float ammoRecovery;
    [SerializeField] float criticalChance;

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
    [SerializeField] float ragdollPushStrenght;

    [Header("Decals")]
    [SerializeField] GameObject bulletHoleDecal;
    [SerializeField] GameObject bloodSplatterDecal;

    [Header("References")]
    [SerializeField] Transform muzzle;
    [SerializeField] LayerMask hitLayer;
    [SerializeField] Animator visualAnimator;

    int currentAmmo; int ammoInMag; 
    float spread; float targetSpread; float currentFireSpread; float currentAirborneSpread; float fireCooldown;
    bool hitTarget; bool activeReload;

    FirstPersonCamera fpsCam; AudioSource audioSource; PlayerHUD hud; WeaponHandler weaponHandler; Rigidbody rb;

    [HideInInspector] public bool shooting; [HideInInspector] public bool reloading;

    /// <summary>
    /// Enumerators
    /// </summary>

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

    /// <summary>
    /// Shooting
    /// </summary>

    public void PlayerTriggerPush()
    {
        if (GetCurrentAmmoMode() > 0)
        {
            ResetReloadState();
        }     

        if (!shooting)
        {
            if (GetCurrentAmmoMode() > 0)
            {
                StartCoroutine(Shoot());
            }
            else
            {
                PlayerReload();
            }
        }
    }

    IEnumerator Shoot()
    {
        shooting = true;
        hitTarget = false;

        reloading = false;
        StopCoroutine(MagReload());
        StopCoroutine(SingleReload());
        GameManager.Instance.playerHUD.reloadText.SetActive(false);

        float burstShots = GetBurstFireShots();

        StartCoroutine(ShootLogic());

        if (visualAnimator != null)
        {
            visualAnimator.SetTrigger("WeaponFire");
        }

        FireKnockback();

        fireCooldown = rateOfFire;

        yield return new WaitForSeconds(fireCooldown);

        StartCoroutine(ManualBoltAction());

        IEnumerator ShootLogic()
        {
            for (int i = 0; i < burstShots; i++)
            {
                if (GetCurrentAmmoMode() > 0)
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
        }

        void ShootBullet()
        {
            for (int i = 0; i < numberOfProyectiles; i++)
            {
                Vector3 shotOrigin = MousePositionSpreadOffset(); Quaternion cameraRotation = Camera.main.transform.rotation;
                Ray ray = Camera.main.ScreenPointToRay(shotOrigin); RaycastHit hit;

                ShootRaycast();
                SpawnBulletTracer();
                HitMarkerHandling();

                void ShootRaycast()
                {
                    Vector3 rayOrigin = Vector3.zero;

                    if (rayOrigin != Vector3.zero)
                    {
                        ray.origin = rayOrigin;
                    }

                    if (Physics.Raycast(ray, out hit, range, hitLayer, QueryTriggerInteraction.Ignore))
                    {
                        CharacterNPC character = hit.transform.GetComponent<CharacterNPC>();

                        if (character != null)
                        {
                            character.RecieveDamageParameters(damageParameters);
                            character.SetLastHitPush(hit.point, ray.direction, ragdollPushStrenght);
                            Instantiate(bloodSplatterDecal, hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
                            hitTarget = true;
                        }
                        else
                        {
                            if (hit.transform.gameObject.layer == 7)
                            {
                                //hit.transform.gameObject.SendMessage("TakeDamage", damage);
                                hit.transform.gameObject.GetComponent<CharacterNPC>().RecieveDamageParameters(damageParameters);
                            }

                            Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                            Instantiate(bulletHoleDecal, hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
                            rayOrigin = hit.point;
                        }
                    }
                }

                void SpawnBulletTracer()
                {
                    if (bulletTracer != null)
                    {
                        GameObject tracer = Instantiate(bulletTracer, muzzle.position, cameraRotation);
                        Vector3 hitPoint = ray.origin + (ray.direction.normalized * range);
                        tracer.GetComponent<BulletTracer>().SetDestination(hitPoint);
                    }
                }

                void HitMarkerHandling()
                {
                    if (hitTarget)
                    {
                        GameManager.Instance.playerHUD.HitMarkerActive();
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

        void FireKnockback()
        {
            if (!GameManager.Instance.playerController.OnGround())
                rb.AddForce(-Camera.main.transform.forward * knockback, ForceMode.Impulse);
        }
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

    void SetCrosshairVisibility()
    {
        try
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
        catch { }
    }

    /// <summary>
    /// Fire Recoil/Spread
    /// </summary>

    void BulletSpreadControl()
    {
        if (hasSpread)
        {
            targetSpread = baseSpread + FireSpread() + AirborneSpread();
            spread = Mathf.Lerp(spread, targetSpread, spreadMultiplier);
            spread = Mathf.Clamp(spread, 0, maxSpread);
            
            GameManager.Instance.crosshairHandler.SetCrosshairSpread(spread);
        }

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

    /// <summary>
    /// Reload Related
    /// </summary>

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
        activeReload = true;

        GameManager.Instance.playerHUD.reloadText.SetActive(true);

        if (visualAnimator != null)
        {
            visualAnimator.SetBool("WeaponReload", true);
        }

        yield return new WaitForSeconds(reloadSpeed);

        if (visualAnimator != null)
        {
            visualAnimator.SetBool("WeaponReload", false);
        }

        if (activeReload)
        {
            ReloadCalculation();
        }

        ResetReloadState();
    }

    IEnumerator SingleReload()
    {
        reloading = true;

        GameManager.Instance.playerHUD.reloadText.SetActive(true);

        int bulletsToReload = magSize - ammoInMag;

        activeReload = true;

        for (int i = 0; i < bulletsToReload; i++)
        {
            if (currentAmmo > 0)
            {
                if (activeReload)
                {
                    ammoInMag++;
                    currentAmmo--;
                    StartCoroutine(weaponHandler.DisplayAmmo());
                    yield return new WaitForSeconds(reloadSpeed);
                }
                else
                {
                    break;
                }
            }
        }

        reloading = false;
        activeReload = false;

        GameManager.Instance.playerHUD.reloadText.SetActive(false);
    }

    void ScriptReload()
    {
        if (ammoMode.ToString() == "Reload")
        {
            ReloadCalculation();
            weaponHandler.DisplayAmmo2();
        }
    }

    void ResetReloadState()
    {
        reloading = false;
        activeReload = false;

        if (visualAnimator != null)
        {
            visualAnimator.SetBool("WeaponReload", false);
        }

        StartCoroutine(weaponHandler.DisplayAmmo());

        GameManager.Instance.playerHUD.reloadText.SetActive(false);
    }

    void ReloadCalculation()
    {
        currentAmmo += ammoInMag;
        ammoInMag = 0;

        int ammo = currentAmmo - magSize;
        ammo = Mathf.Clamp(ammo, 0, magSize);

        int ammoToMag = currentAmmo - ammo;
        ammoToMag = Mathf.Clamp(ammoToMag, 0, magSize);

        ammoInMag = ammoToMag;
        currentAmmo -= ammoToMag;
    }

    /// <summary>
    /// Ammunition Control
    /// </summary>

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

    public void AmmoRefill(float scriptAmmoRecovery)
    {
        float ammoToRecover = 0;

        switch (ammoRecovery)
        {
            case 0:
                ammoToRecover = ammoRecovery;
                break;

            default:
                ammoToRecover = scriptAmmoRecovery;
                break;
        }

        int ammoToRefill = (int)(totalAmmo * ammoToRecover) / 100;
        ammoToRefill = Mathf.Clamp(ammoToRefill, 1, 3000);
        int ammoGet = currentAmmo + ammoToRefill;
        ammoGet = Mathf.Clamp(ammoGet, 1, totalAmmo);
        currentAmmo = ammoGet;
        ScriptReload();
    } 

    /// <summary>
    /// Unity Methods
    /// </summary>

    void OnEnable()
    {
        GetReferences();

        SetCrosshairVisibility();

        StartCoroutine(weaponHandler.DisplayAmmo());

        if (visualAnimator != null)
        {
            visualAnimator.SetBool("WeaponReload", false);
        }
    }

    void GetReferences()
    {
        weaponHandler = transform.parent.GetComponent<WeaponHandler>();
        fpsCam = transform.parent.parent.GetComponent<FirstPersonCamera>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        rb = GameManager.Instance.playerController.rb;
        currentAmmo = totalAmmo;
        ScriptReload();
    }

    void Update()
    {
        BulletSpreadControl();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Ray ray = Camera.main.ScreenPointToRay(MousePositionSpreadOffset());
        Gizmos.DrawRay(ray);
    }
}
 