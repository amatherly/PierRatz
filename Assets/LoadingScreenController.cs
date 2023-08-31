using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField, Tooltip("Reference to the Slider UI component")]
    private Slider progressBar;

    private static int sceneToLoad = -1;

    private void Start()
    {
        if (sceneToLoad != -1)
        {
            StartCoroutine(LoadSceneAsync());
        }
        else
        {
            Debug.LogError("sceneToLoad is not set. Cannot proceed with loading.");
        }
    }

    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            progressBar.value = progress;

            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public static void LoadScene(int buildIndex)
    {
        sceneToLoad = buildIndex;
        SceneManager.LoadScene("LoadingScreen");
    }

    public void LoadNextScene()
    {
        int nextSceneToLoad = (SceneManager.GetActiveScene().buildIndex + 1);
        LoadScene(nextSceneToLoad);
    }
}