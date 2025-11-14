using UnityEngine;

public class Misile : MonoBehaviour
{
    [SerializeField] float misileSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * misileSpeed * Time.deltaTime;
    }
}
