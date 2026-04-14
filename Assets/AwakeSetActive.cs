using UnityEngine;
using UnityEngine.SceneManagement;

public class AwakeSetActive : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] GameObject otherGameObject;

    private void Awake()
    {
        if (Time.timeSinceLevelLoad < 1) 
        {
            if (otherGameObject == null)
            {
                gameObject.SetActive(active);
            }
            else
            {
                otherGameObject.SetActive(active);
            }
        }        
    }
}
