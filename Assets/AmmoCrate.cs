using UnityEngine;

public class AmmoCrate : MonoBehaviour
{
    void TakeAmmo()
    {

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
