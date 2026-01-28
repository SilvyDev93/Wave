using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.Instance.UnPauseGame();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void Awake()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }        
    }
}
