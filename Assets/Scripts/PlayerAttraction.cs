using UnityEngine;

public class PlayerAttraction : MonoBehaviour
{
    Transform targetObject;
    Transform playerTransform;

    void FollowPlayer()
    {
        targetObject.position = Vector3.MoveTowards(targetObject.position, playerTransform.position, 0.5f);
    }

    void Update()
    {
        FollowPlayer();
    }

    private void Awake()
    {
        playerTransform = GameManager.Instance.playerCharacter.transform;

        if (transform.root != transform)
        {
            targetObject = transform.root;
        }
        else
        {
            targetObject = transform;
        }
    }
}
