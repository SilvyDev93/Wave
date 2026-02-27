using System.Collections;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] float respawnTime;
    [SerializeField] MeshRenderer meshRenderer; 
    
    Collider col;
    
    public void TakeDamage(int damage)
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        SetActive(false);
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        SetActive(true);
    }

    void SetActive(bool active)
    {
        meshRenderer.enabled = active;
        col.enabled = active;
    }

    private void Awake()
    {
        col = GetComponent<Collider>();
    }
}
