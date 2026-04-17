using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
    [SerializeField] Slider loadBarSlider;

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2);

        while (!asyncLoad.isDone)
        {
            Debug.Log(asyncLoad.progress);
            loadBarSlider.value = asyncLoad.progress;
            yield return null;
        }
    }

    void Awake()
    {
        StartCoroutine(LoadSceneAsync());
    }
}
