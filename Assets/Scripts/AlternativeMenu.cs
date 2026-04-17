using UnityEngine;

public class AlternativeMenu : MonoBehaviour
{
    [SerializeField] GameObject menuToReturn;

    public void ChangeToAlternativeMenu()
    {
        menuToReturn.SetActive(false);
        gameObject.SetActive(true);
    }

    public void ReturnToPreviousMenu()
    {
        menuToReturn.SetActive(true);
        gameObject.SetActive(false);
    }
}
