using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Parameters")]
    [SerializeField] float damage;
    [SerializeField] float fireRate;
    [SerializeField] int ammo;
    [SerializeField] int range;
    [SerializeField] float verticalRecoil;
    [SerializeField] float horizontalRecoil;
    [SerializeField] bool automatic;

    [Header("References")]
    [SerializeField] PlayerHUD hud;
    [SerializeField] CameraShake cameraShake;

    float fireCooldown; bool shooting;

    AudioSource audioSource;
   
    IEnumerator Shoot()
    {
        shooting = true;

        yield return new WaitForSeconds(fireCooldown);
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

        transform.parent.parent.GetComponent<FirstPersonCamera>().FireRecoil(verticalRecoil, horizontalRecoil);
        cameraShake.StartShake(1, 1, 0.2f);
        GameManager.Instance.audioManager.PlayAudioPitch(audioSource, Random.Range(0.8f, 1.2f));

        ammo--;
        hud.ammoCounter.text = ammo.ToString();
        fireCooldown = fireRate;

        shooting = false;
    }

    void PlayerInput()
    {
        switch (automatic)
        {
            case true:

                if (Input.GetMouseButton(0))
                {
                    PlayerTriggerPush();
                }

                break;

            case false:

                if (Input.GetMouseButtonDown(0))
                {
                    PlayerTriggerPush();
                }

                break;
        }

        void PlayerTriggerPush()
        {
            if (!shooting && ammo > 0)
            {
                StartCoroutine(Shoot());
            }
        }       
    }

    void Update()
    {
        PlayerInput();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        hud.ammoCounter.text = ammo.ToString();
    }
}
 