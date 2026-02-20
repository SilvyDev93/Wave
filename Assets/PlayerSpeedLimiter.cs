using TMPro;
using UnityEngine;

public class PlayerSpeedLimiter : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float wallPushStrengh;
    [SerializeField] float lockedInputDuration;

    Vector3 lastPosition;

    bool updatePostion = true;

    [HideInInspector] public bool triggerActive = true;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerActive)
        {
            if (other.gameObject.layer == 3)
            {
                updatePostion = false;
                lastPosition.y = GameManager.Instance.playerController.transform.position.y;
                GameManager.Instance.playerController.transform.position = lastPosition;
                StopCoroutine(GameManager.Instance.playerInput.TempLockedMovement(lockedInputDuration));
                StartCoroutine(GameManager.Instance.playerInput.TempLockedMovement(lockedInputDuration));
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (triggerActive)
        {
            if (other.gameObject.layer == 3)
            {
                GameManager.Instance.playerController.rb.AddForce(GetCollisionOutwardDirection(other) * wallPushStrengh, ForceMode.Impulse);
            }
        }       
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerActive)
        {
            if (other.gameObject.layer == 3)
            {
                updatePostion = true;
                Vector3 vel = GameManager.Instance.playerController.rb.linearVelocity;
                vel.x = 0;
                vel.z = 0;
                GameManager.Instance.playerController.rb.linearVelocity = vel;
            }
        }    
    }

    private void Update()
    {
        if (updatePostion)
        {
            lastPosition = GameManager.Instance.playerController.transform.position;
        }       
    }

    Vector3 GetCollisionOutwardDirection(Collider col)
    {
        Vector3 dir = (col.transform.position - transform.position).normalized;
        return -col.transform.forward;
    }
}
