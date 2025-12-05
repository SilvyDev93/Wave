using UnityEngine;

public class CorpseObject : MonoBehaviour
{
    public void PushCorpse(Vector3 point, Vector3 direction, float push)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.AddForceAtPosition(direction * push * 10, point);
    }
}
