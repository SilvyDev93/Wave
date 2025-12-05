using UnityEngine;

public class CorpseObject : MonoBehaviour
{
    public void PushCorpse(Vector3 point, Vector3 normal, float push)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.AddForceAtPosition(transform.forward * push, point);
    }
}
