using UnityEngine;

public class Saw : Proyectile
{
    public override void ProyectileImpact(Collider collider)
    {
        if (collider.gameObject.layer == 3 || collider.gameObject.layer == 9)
        {
            SendBackSaw();
        }

        if (collider.gameObject.layer == 7)
        {
            if (collider.gameObject.tag != "Player")
            {
                CharacterNPC character = collider.transform.GetComponent<CharacterNPC>();

                if (character != null)
                {
                    //collision.gameObject.SendMessage("TakeDamage", directHitDamage);
                    collider.gameObject.GetComponent<CharacterNPC>().RecieveDamageParameters(damageParameters);
                }
                else
                {
                    collider.transform.SendMessage("TakeDamage", damageParameters.minDamage);
                }
            }
        }
    }

    void SendBackSaw()
    {
        proyectileSpeed *= -1;
    }
}
