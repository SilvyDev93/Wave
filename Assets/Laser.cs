using UnityEngine;

public class Laser : Proyectile
{
    public override void ProyectileImpact(Collider collider)
    {
        if (collider.gameObject.layer == 3 || collider.gameObject.layer == 9)
        {
            Debug.Log("Destroyeee");
            Destroy(gameObject);
        }

        if (collider.gameObject.layer == 7)
        {
            Debug.Log(collider.gameObject.name);

            if (collider.gameObject.tag == "Player")
            {
                PlayerCharacter character = collider.transform.GetComponent<PlayerCharacter>();

                if (character != null)
                {
                    //collision.gameObject.SendMessage("TakeDamage", directHitDamage);
                    character.TakeDamage(damageParameters.minDamage);
                }
            }
        }
    }
}
