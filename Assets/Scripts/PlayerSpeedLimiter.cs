using UnityEngine;

public class PlayerSpeedLimiter : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float wallPushStrengh;
    [SerializeField] float lockedInputDuration;

    [Header("References")]
    [SerializeField] GameObject collisionDetector;

    Ray ray;

    [HideInInspector] public bool triggerActive = true;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerActive)
        {
            if (other.gameObject.layer == 3)
            {
                StopCoroutine(GameManager.Instance.playerInput.TempLockedMovement(lockedInputDuration));
                StartCoroutine(GameManager.Instance.playerInput.TempLockedMovement(lockedInputDuration));

                GameManager.Instance.playerController.rb.AddForce(GetCollisionOutwardDirection(other) * wallPushStrengh, ForceMode.VelocityChange);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerActive)
        {
            if (other.gameObject.layer == 3)
            {
                Vector3 vel = GameManager.Instance.playerController.rb.linearVelocity;
                vel.x = 0;
                vel.z = 0;
                GameManager.Instance.playerController.rb.linearVelocity = vel;             
            }
        }    
    }

    Vector3 GetCollisionOutwardDirection(Collider col)
    {
        GameObject colDetector = Instantiate(collisionDetector);
        colDetector.transform.position = col.ClosestPointOnBounds(transform.position);

        RaycastHit hit;
        Vector3 direction = transform.position - col.ClosestPointOnBounds(transform.position);
        ray = new Ray(transform.position, direction);
        Vector3 normalDirection = Vector3.zero;

        if (Physics.Raycast(ray, out hit, 10, 3, QueryTriggerInteraction.Ignore))
        {
            normalDirection = hit.normal;
        }
        
        return normalDirection.normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(ray);
    }
}
