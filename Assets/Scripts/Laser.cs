using UnityEngine;

public class Laser : Proyectile
{
    public override void ProyectileImpact(Collider collider)
    {
        if (collider.gameObject.layer == 3 || collider.gameObject.layer == 9)
        {
            Destroy(gameObject);
        }

        if (collider.gameObject.layer == 7)
        {
            if (collider.gameObject.tag == "Player")
            {
                PlayerCharacter character = collider.transform.GetComponent<PlayerCharacter>();

                if (character != null)
                {
                    character.TakeDamage(damageParameters.minDamage);
                }
            }
        }
    }
}
