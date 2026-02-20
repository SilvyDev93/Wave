using UnityEngine;

public class Misile : Proyectile
{
    [Header("Misile Parameters")]
    [SerializeField] GameObject explosion;
    [SerializeField] float directHitDamage;

    public override void ProyectileImpact(Collision collision)
    {
        if (collision.gameObject.layer == 3 || collision.gameObject.layer == 9)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == 7)
        {
            if (collision.gameObject.tag != "Player")
            {
                //collision.gameObject.SendMessage("TakeDamage", directHitDamage);
                collision.gameObject.GetComponent<CharacterNPC>().RecieveDamageParameters(damageParameters);
                Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
