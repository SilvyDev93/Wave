using System.Collections;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] bool destroyFromStart = true;

    IEnumerator TimeCountdown()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    public void StartObjectDestruction()
    {        
        StartCoroutine(TimeCountdown());
    }

    void Start()
    {       
        if (destroyFromStart)
        {
            StartObjectDestruction();
        }
    }
}
