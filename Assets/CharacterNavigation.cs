using UnityEngine;
using UnityEngine.AI;

public class CharacterNavigation : MonoBehaviour
{
    NavMeshAgent agent;

    private void Update()
    {
        agent.destination = GameManager.Instance.playerCharacter.transform.position;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }
}
