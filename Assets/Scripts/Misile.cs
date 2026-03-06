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
                // escudo tankear misil petar con el tiempo

                var character = collision.gameObject.GetComponent<CharacterNPC>();

                if (character != null)
                {
                    character.RecieveDamageParameters(damageParameters);
                }
                
                Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
