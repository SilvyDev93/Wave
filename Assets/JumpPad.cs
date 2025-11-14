using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] float impulseForce;

    void OnTriggerEnter(Collider other)
    {
        Vector2 force = new Vector2(0, impulseForce);
        other.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }
}
