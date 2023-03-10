using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameOverModal : MonoBehaviour
{
    public TextMeshProUGUI GameOverText;
    [Inject] private SceneLoader SceneLoader;
    public void Show(int score) {
        gameObject.SetActive(true);
        GameOverText.text = "Final score: " + score;
    }
    public void RestartButton() {
        SceneLoader.Restart();
    }
}
