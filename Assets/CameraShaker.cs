using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [Header("Shake Configuration")]
    [SerializeField] float strenght;
    [SerializeField] float initialSpeed;
    [SerializeField] float smoothTime;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            float playerDistance = Vector3.Distance(transform.parent.position, GameManager.Instance.playerCharacter.transform.position);
            Camera.main.gameObject.GetComponent<FirstPersonCamera>().StartShake(strenght - playerDistance, initialSpeed, smoothTime);
        }
    }
}
