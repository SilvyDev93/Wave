using System.Collections;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] float lifeTime;

    IEnumerator TimeCountdown()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(TimeCountdown());
    }
}
