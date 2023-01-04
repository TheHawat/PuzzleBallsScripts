using UnityEngine;
using Zenject;
public class BallsManager : MonoBehaviour
{
    [InjectOptional(Id = "BallPrefab")] public GameObject ballPrefab;
    [InjectOptional(Id = "Parent")] public GameObject Parent;
    [Inject] SceneConfig SC;
    [Inject] private IAudioManager _speaker;
    public static int NumberOfBalls = 0;
    public BallBehaviour CreateBall(Vector2 Position) {
        GameObject ball = Instantiate(ballPrefab, Position, Quaternion.identity, Parent.transform);
        NumberOfBalls++;
        ball.name = $"Ball {NumberOfBalls}";
        Color _randomCol = SC.BallColors[Random.Range(0, SC.BallColors.Length)];
        ball.GetComponent<SpriteRenderer>().color = _randomCol;
        BallBehaviour _ballObject = ball.GetComponent<BallBehaviour>();
        _ballObject.Index = NumberOfBalls;
        _ballObject.Speaker = _speaker;
        return _ballObject;
    }
}
