using UnityEngine;

public class Proyectile : MonoBehaviour
{
    [Header("Proyectile Base")]
    [SerializeField] protected float proyectileSpeed;
    [SerializeField] protected DamageParameters damageParameters;

    public virtual void ProyectileImpact(Collision collision)
    {
        // Impact
    }

    public virtual void ProyectileMovement()
    {
        transform.position += transform.forward * proyectileSpeed * Time.deltaTime;
    }

    void IgnorePlayerCollision()
    {
        GameObject playerCharacter = GameManager.Instance.playerCharacter.gameObject;
        Physics.IgnoreCollision(playerCharacter.GetComponent<Collider>(), GetComponent<Collider>());
    }

    public void SetDamageParameters(DamageParameters givenParameters)
    {
        damageParameters = givenParameters;
    }

    void OnCollisionEnter(Collision collision)
    {
        ProyectileImpact(collision);
    }

    void Update()
    {
        ProyectileMovement();
    }

    void Start()
    {
        IgnorePlayerCollision();
    }
}
