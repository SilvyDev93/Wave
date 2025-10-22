using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float fireRate;
    [SerializeField] int ammo;

    [SerializeField] PlayerHUD hud;
    [SerializeField] CameraShake cameraShake;
    AudioSource audioSource;

    float fireCooldown;

    bool shooting;

    IEnumerator Shoot()
    {
        shooting = true;
        yield return new WaitForSeconds(fireCooldown);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.Log(hit.transform.gameObject.name);
        }

        cameraShake.StartShake(1, 1, 0.2f);
        audioSource.Play();

        ammo--;
        fireCooldown = fireRate;
        shooting = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !shooting && ammo > 0)
        {
            StartCoroutine(Shoot());
        }

        hud.ammoCounter.text = ammo.ToString();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
 