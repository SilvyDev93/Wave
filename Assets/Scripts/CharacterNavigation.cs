using UnityEngine;
using UnityEngine.AI;

public class CharacterNavigation : MonoBehaviour
{
    NavMeshAgent agent;

    public void DisableAgent()
    {
        agent.enabled = false;
    }

    private void Update()
    {
        if (agent.enabled) 
        {
            agent.destination = GameManager.Instance.playerCharacter.transform.position;
        }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();        
    }
}
