using UnityEngine;

public class Misile : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] float misileSpeed;
    
    void Update()
    {
        transform.position += transform.forward * misileSpeed * Time.deltaTime;
    }
   
    void Start()
    {
        GameObject playerCharacter = GameManager.Instance.playerCharacter.gameObject;
        Physics.IgnoreCollision(playerCharacter.GetComponent<Collider>(), GetComponent<Collider>());
    }

    void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
