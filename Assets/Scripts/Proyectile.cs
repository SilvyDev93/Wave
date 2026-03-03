using UnityEngine;

public class Proyectile : MonoBehaviour
{
    [Header("Proyectile Base")]
    [SerializeField] protected float proyectileSpeed;
    [SerializeField] protected DamageParameters damageParameters;
    [SerializeField] bool ignoresPlayerCollision = true;
    [SerializeField] bool ignoresNpcCollision;

    Rigidbody rb;

    public virtual void ProyectileImpact(Collision collision)
    {
        // Impact
        CheckNpcCollisionValidity(collision);
    }

    public virtual void ProyectileImpact(Collider collider)
    {
        // Impact
        CheckNpcCollisionValidity(collider);
    }

    public virtual void ProyectileMovement()
    {
        //transform.position += transform.forward * proyectileSpeed * Time.deltaTime;
        rb.linearVelocity = transform.forward * proyectileSpeed * Time.deltaTime;
    }

    void IgnorePlayerCollision()
    {
        if (ignoresPlayerCollision) 
        {
            GameObject playerCharacter = GameManager.Instance.playerCharacter.gameObject;
            Physics.IgnoreCollision(playerCharacter.GetComponent<Collider>(), GetComponent<Collider>());
        }        
    }

    void CheckNpcCollisionValidity(Collision collision)
    {
        if (ignoresNpcCollision)
        {
            if (collision.gameObject.GetComponent<CharacterNPC>() != null)
            {
                Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            }
        }
    }

    void CheckNpcCollisionValidity(Collider collider)
    {
        if (ignoresNpcCollision)
        {
            if (collider.gameObject.GetComponent<CharacterNPC>() != null)
            {
                Physics.IgnoreCollision(collider, GetComponent<Collider>());
            }
        }
    }

    public void SetDamageParameters(DamageParameters givenParameters)
    {
        damageParameters = givenParameters;
    }

    void OnCollisionEnter(Collision collision)
    {
        ProyectileImpact(collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        ProyectileImpact(other);
    }

    void Update()
    {
        ProyectileMovement();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        IgnorePlayerCollision();
    }
}
