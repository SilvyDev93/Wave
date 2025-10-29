using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject playerHUD;
    [SerializeField] GameObject pauseMenu;

    public void PauseMenuHandler()
    {
        switch (pauseMenu.activeSelf)
        {
            case true:
                playerHUD.SetActive(true);
                pauseMenu.SetActive(false);
                GameManager.Instance.UnPauseGame();
                break;

            case false:
                playerHUD.SetActive(false);
                pauseMenu.SetActive(true);
                GameManager.Instance.PauseGame();
                break;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void Awake()
    {
        pauseMenu.SetActive(false);
    }
}
