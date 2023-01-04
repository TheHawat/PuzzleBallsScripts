using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public float FADE_TIME = 0.3f;
    public GameObject loadingScreen;
    public CanvasGroup canvasGroup;
    private static GameObject instance;
    private string CurrentScene = "Main Menu";
    public void Start() {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);
    }
    public void Restart() {
        StartCoroutine(StartLoad(CurrentScene));
    }
    public void StartEndless() {
        StartCoroutine(StartLoad("SampleScene"));
    }
    public void GoToMainMenu() {
        StartCoroutine(StartLoad("Main Menu"));
    }
    IEnumerator StartLoad(string sceneName) {
        loadingScreen.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreen(1.0f, FADE_TIME));
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone) {
            yield return null;
        }
        yield return StartCoroutine(FadeLoadingScreen(0.0f, FADE_TIME));
        loadingScreen.SetActive(false);
        CurrentScene = sceneName;
    }
    IEnumerator FadeLoadingScreen(float targetValue, float duration) {
        float startValue = canvasGroup.alpha;
        float time = 0;
        while (time < duration) {
            canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = targetValue;
    }
}