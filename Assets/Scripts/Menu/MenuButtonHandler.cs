using UnityEngine;

public class MenuButtonHandler : MonoBehaviour
{
    [SerializeField] GameObject selectionGO;
    MenuAudioHandler audioHandler;

    public void OnButtonHover()
    {
        audioHandler.PlaySelectionSound();
        selectionGO.SetActive(true);
    }

    public void OffButtonHover()
    {       
        selectionGO.SetActive(false);
    }

    public void OnButtonPress()
    {
        audioHandler.PlayPressSound();
    }

    void OnDisable()
    {
        selectionGO.SetActive(false);
    }

    void Awake()
    {
        audioHandler = FindAnyObjectByType<MenuAudioHandler>();
        selectionGO.SetActive(false);
    }
}
