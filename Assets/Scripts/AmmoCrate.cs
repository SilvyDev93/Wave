using UnityEngine;

public class AmmoCrate : MonoBehaviour
{
    [SerializeField] float ammoRecovery;

    void TakeAmmo()
    {
        Transform weaponHandler = GameManager.Instance.weaponHandler.transform;
        int numberRNG = Random.Range(0, weaponHandler.childCount);

        Weapon weaponToReload = GameManager.Instance.weaponHandler.transform.GetChild(numberRNG).GetComponent<Weapon>();
        weaponToReload.AmmoRefill(ammoRecovery);

        PlayerHUD hud = GameManager.Instance.playerHUD;

        hud.SetPlayerMessage("Got " + weaponToReload.name + " ammo", 2);

        PlayerSounds playerSounds = GameManager.Instance.audioManager.playerSounds;
        playerSounds.PlayAudio("ammoRecovery");

        Destroy(transform.parent.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            TakeAmmo();
        }
    }
}
