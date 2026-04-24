using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] bool followX;
    [SerializeField] bool followY;
    [SerializeField] bool followZ;

    Transform playerTransform;

    private void Update()
    {
        Vector3 newPosition = playerTransform.position;

        if (followX)
        {
            newPosition.x = playerTransform.position.x;
        }
        else
        {
            newPosition.x = transform.position.x;
        }

        if (followY)
        {
            newPosition.y = playerTransform.position.y;
        }
        else
        {
            newPosition.y = transform.position.y;
        }

        if (followZ)
        {
            newPosition.z = playerTransform.position.z;
        }
        else
        {
            newPosition.z = transform.position.z;
        }

        transform.position = newPosition;
    }

    private void Start()
    {
        playerTransform = GameManager.Instance.playerCharacter.transform;
    }
}
