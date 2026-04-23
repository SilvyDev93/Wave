using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    Transform playerTransform;

    private void Update()
    {
        Vector3 newPosition = playerTransform.position;

        newPosition.x = playerTransform.position.x;
        newPosition.z = playerTransform.position.z;

        transform.position = newPosition;
    }

    private void Start()
    {
        playerTransform = GameManager.Instance.playerCharacter.transform;
    }
}
