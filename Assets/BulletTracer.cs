using UnityEngine;

public class BulletTracer : MonoBehaviour
{
    [SerializeField] float movementSpeed;

    Vector3 target; bool tracerActivated;

    void TracerMovement()
    {
        if (tracerActivated)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, movementSpeed * Time.deltaTime);

            if (transform.position == target)
            {
                Destroy(gameObject);
            }
        }       
    }

    public void SetDestination(Vector3 target)
    {
        this.target = target;
        transform.LookAt(target);
        tracerActivated = true;
    }

    void Update()
    {
        TracerMovement();
    }
}
