using UnityEngine;
using UnityEngine.AI;

public class CharacterNavigation : MonoBehaviour
{
    NavMeshAgent agent;

    public void SetAgentActive(bool active)
    {
        agent.enabled = active;
    }

    private void Update()
    {
        if (agent.enabled) 
        {
            agent.destination = GameManager.Instance.playerCharacter.transform.position;
        }
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();        
    }
}
