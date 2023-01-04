using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager : MonoBehaviour
{
    public Traversal _traversal;
    public BGrid _grid;
    public GameOverModal GameOverModal;
    public IndicatorBehaviour Indicator;
    public ScoreManager ScoreManager;
    public BallsManager BallsManager;
    public BoxCollider2D BalistaBox;
    [Inject] private IAudioManager _speaker;
    [Inject] private SceneConfig _sceneConfig;
    [Inject] private ProjectConfig _projectConfig;
    [Inject] private SceneLoader SceneLoader;
    private int _shootCounter = 0;
    private bool _canShoot = false;
    private BallBehaviour _currentBall;
    private BallBehaviour _nextBall;
    public GameObject Ceiling;

    public void Start() {
        InitiliseGameObjects();
        InitialiseGrid();
    }
    public void Update() {
        if (Input.GetKeyDown(_projectConfig.ResetKey)) ResetGame();
        if (!_canShoot) return;
        if (ReadyToShoot()) Shoot();
    }

    private bool ReadyToShoot() {
        if (Input.GetKey(_projectConfig.InteractKey)) return true;
        if (Input.touchCount > 0) {
            if (Input.touches[0].phase == TouchPhase.Began) {
                Vector3 touchPos = Input.touches[0].position;
                Ray ray = Camera.main.ScreenPointToRay(touchPos);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                if (hit.collider == BalistaBox) return true;
            }
        }
        return false;
    }

    private void Shoot() {
        _canShoot = false;
        _shootCounter++;
        Vector2 _startingVelocity = Indicator.CalcStartingVelocity();
        _currentBall.Launch(_startingVelocity);
    }

    private void InitialiseGrid() {
        for (int i = 0; i < _sceneConfig.FilledRows; i++) {
            FillBottomRowWithNewBalls();
        }
    }
    private void InitiliseGameObjects() {
        _currentBall = CreateBall(_sceneConfig.CurrentBallPosition);
        _nextBall = CreateBall(_sceneConfig.NextBallPosition);
        _canShoot = true;
        _speaker.PlayJazz();
    }
    private void ResetGame() {
        SceneLoader.StartGame();
    }
    private void CeilingDropCheck() {
        if (_shootCounter >= _sceneConfig.TTL) {
            _sceneConfig.TTL = Math.Max(_sceneConfig.TTL - 1, 2);
            _shootCounter = 0;
            if (_sceneConfig.GameMode == "Endless") DropEndless();
            else if (_sceneConfig.GameMode == "Prebuilt") DropPrebuilt();
        }
    }

    private void DropEndless() {
        _grid.CreateNewRowTop();
        FillTopRowWithNewBalls();
    }
    private void DropPrebuilt() {
        _grid.LowerBallsPrebuilt();
        Vector2 Position = Ceiling.transform.position;
        Position.y -= _projectConfig.RowHeight;
        Ceiling.transform.position = Position;
        _sceneConfig.AllowedRows--;
    }


    private void GameOverCheck() {
        if (_grid.LowestRow() >= _sceneConfig.AllowedRows) {
            _canShoot = false;
            int score = ScoreManager.Score;
            GameOverModal.Show(score);
        }
    }
    private void FillBottomRowWithNewBalls() {
        FillRowWithNewBalls(_grid.LowestRow() + 1);
    }
    private void FillTopRowWithNewBalls() {
        FillRowWithNewBalls(0);
    }
    private void FillRowWithNewBalls(int row) {
        _canShoot = false;
        for (int i = 0; i < _grid.Grid[row].Length; i++) {
            BallBehaviour ball = CreateBall(_grid.GridPositions[row][i]);
            _grid.AddBall(row, i, ball.UnderlyingObject());
        }
        _canShoot = true;
    }
    internal BallBehaviour CreateBall(Vector2 Position) {
        BallBehaviour _ballObject = BallsManager.CreateBall(Position);
        _ballObject.OnCeilingHit += handleCelingHit;
        return _ballObject;
    }
    internal void handleCelingHit(Vector2 position) {
        (int i, int j) = _grid.FindNearest(position);
        GameObject _go = _currentBall.UnderlyingObject();
        _grid.AddBall(i, j, _go);
        _speaker.PlayTheSoundOf("Ceiling");
        _currentBall.Freeze(_grid.GridPositions[i][j]);
        _currentBall = _nextBall;
        _currentBall.Reposition(_sceneConfig.CurrentBallPosition);
        _nextBall = CreateBall(_sceneConfig.NextBallPosition);
        _canShoot = true;
        int Removed = _traversal.CheckForRemoval(i, j);
        int Hanging = _traversal.DropHanging();
        ScoreManager.AddScore(Removed, Hanging);
        CeilingDropCheck();
        GameOverCheck();
        _grid.TrimEmptyRows();
    }
}
