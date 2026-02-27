using Palmmedia.ReportGenerator.Core.CodeAnalysis;
using TMPro;
using UnityEngine;

public class PlayerSpeedLimiter : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float wallPushStrengh;
    [SerializeField] float lockedInputDuration;
    [SerializeField] bool teleportPlayer;

    [Header("References")]
    [SerializeField] GameObject collisionDetector;

    Vector3 lastPosition; Ray ray; Ray ray2; 

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

                if (teleportPlayer)
                {
                    GameManager.Instance.playerController.transform.position = lastPosition;
                }
                
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
        GameObject colDetector = Instantiate(collisionDetector);
        colDetector.transform.position = col.ClosestPointOnBounds(transform.position);

        RaycastHit hit;
        Vector3 direction = transform.position - col.ClosestPointOnBounds(transform.position);
        ray = new Ray(transform.position, direction);
        Vector3 normalDirection = Vector3.zero;

        if (Physics.Raycast(ray, out hit, 10, 3, QueryTriggerInteraction.Ignore))
        {
            normalDirection = transform.InverseTransformDirection(hit.normal);
        }
        
        return normalDirection;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(ray);
    }
}
