using UnityEngine;

public class PlayerSpeedLimiter : MonoBehaviour
{
    [SerializeField] float wallPushStrengh;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Debug.Log("Detecting Wall");
            GameManager.Instance.playerController.rb.AddForce(-other.transform.forward * wallPushStrengh);
            GameManager.Instance.playerInput.lockedMovement = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            GameManager.Instance.playerInput.lockedMovement = false;
        }
    }
}
