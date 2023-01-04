using TMPro;
using UnityEngine;
using Zenject;

public class ScoreManager : MonoBehaviour
{
    private int _score = 0;
    public int Score => _score;
    [Inject] private ProjectConfig PC;
    public TextMeshProUGUI ScoreText;
    public void Update() {
        ScoreText.text = "" + _score;
    }
    public void AddScore(int Removed, int Hanging) {
        _score += Removed * PC.PointsForRemoved;
        _score += Hanging * PC.PointsForHanging;
    }
    public void Reset() {
        _score = 0;
    }
}
