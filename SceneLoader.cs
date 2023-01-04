using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public float FADE_TIME = 0.3f;
    public GameObject loadingScreen;
    public string sceneToLoad;
    public CanvasGroup canvasGroup;
    private static GameObject instance;
    public void Start() {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);
    }
    public void StartGame() {
        StartCoroutine(StartLoad());
    }
    IEnumerator StartLoad() {
        loadingScreen.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreen(1.0f, FADE_TIME));
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!operation.isDone) {
            yield return null;
        }
        yield return StartCoroutine(FadeLoadingScreen(0.0f, FADE_TIME));
        loadingScreen.SetActive(false);
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