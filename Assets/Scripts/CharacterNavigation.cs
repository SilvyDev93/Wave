using UnityEngine;
using UnityEngine.AI;

public class CharacterNavigation : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Transform playerTransform;

    Vector3 destination;

    bool followPlayer;

    public void SetAgentActive(bool active)
    {
        agent.enabled = active;
    }

    public void SetAgentDestination(Vector3 destination)
    {
        this.destination = destination;
    }

    public void FollowPlayer(bool active)
    {
        followPlayer = active;
    }

    private void Update()
    {
        if (agent.enabled) 
        {
            switch (followPlayer)
            {
                case true:
                    agent.destination = playerTransform.position;
                    break;

                case false:
                    agent.destination = destination;
                    break;
            }            
        }
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameManager.Instance.playerCharacter.transform;
        followPlayer = true;
    }
}
