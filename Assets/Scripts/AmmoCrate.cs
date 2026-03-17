using UnityEngine;

public class AmmoCrate : MonoBehaviour
{
    [SerializeField] float ammoRecovery;

    Transform weaponHandler;

    void TakeAmmo()
    {
        weaponHandler = GameManager.Instance.weaponHandler.transform;       

        Weapon weaponToReload = GetWeapon();

        weaponToReload.AmmoRefill(ammoRecovery);

        PlayerHUD hud = GameManager.Instance.playerHUD;

        hud.SetPlayerMessage("Got " + weaponToReload.name + " ammo", 2);

        PlayerSounds playerSounds = GameManager.Instance.audioManager.playerSounds;
        playerSounds.PlayAudio("ammoRefill");

        Destroy(transform.parent.gameObject);
    }

    Weapon GetWeapon()
    {
        int numberRNG = Random.Range(0, weaponHandler.childCount);
        Weapon weapon = GameManager.Instance.weaponHandler.transform.GetChild(numberRNG).GetComponent<Weapon>();

        if (weapon.unlimitedAmmo && weaponHandler.childCount > 1)
        {
            return GetWeapon();
        }
        else
        {
            return weapon;
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            TakeAmmo();
        }
    }
}
