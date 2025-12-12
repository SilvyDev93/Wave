using System.Collections;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] float respawnTime;

    MeshRenderer meshRenderer; Collider col;
    
    public void TakeDamage(float damage)
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
        meshRenderer = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
    }
}
