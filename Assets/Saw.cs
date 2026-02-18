using UnityEngine;

public class Saw : Proyectile
{
    public override void ProyectileImpact(Collision collision)
    {
        if (collision.gameObject.layer == 3 || collision.gameObject.layer == 9)
        {
            SendBackSaw();
        }

        if (collision.gameObject.layer == 7)
        {
            if (collision.gameObject.tag != "Player")
            {
                CharacterNPC character = collision.transform.GetComponent<CharacterNPC>();

                if (character != null)
                {
                    //collision.gameObject.SendMessage("TakeDamage", directHitDamage);
                    collision.gameObject.GetComponent<CharacterNPC>().RecieveDamageParameters(damageParameters);
                }
                else
                {
                    collision.transform.SendMessage("TakeDamage", damageParameters.minDamage);
                }
            }
        }
    }

    void SendBackSaw()
    {
        proyectileSpeed *= -1;
    }
}
