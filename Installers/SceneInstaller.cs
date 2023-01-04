using Zenject;
using UnityEngine;
using System.Collections;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private int _gridColumns = 8;
    [SerializeField] private int _allowedRows = 9;
    [SerializeField] private int _filledRows = 2;
    [SerializeField] private int TTL = 10;
    [SerializeField] private Vector2 _currentBallPosition = new(0, -4.9f);
    [SerializeField] private Vector2 _nextBallPosition = new(3.0f, -5.5f);
    [SerializeField] private Color[] _ballCol = { Color.white, Color.red, Color.green, Color.blue };
    [SerializeField] private GameModes _gameMode;
    [SerializeField] private float _angle;

    enum GameModes { Endless, Prebuilt }
    public override void InstallBindings() {
        Container
           .Bind<IAudioManager>()
           .FromComponentInHierarchy()
           .AsSingle();

        SceneConfig SC = new SceneConfig {
            TTL = TTL,
            CurrentBallPosition = _currentBallPosition,
            NextBallPosition = _nextBallPosition,
            GridColumns = _gridColumns,
            AllowedRows = _allowedRows,
            FilledRows = _filledRows,
            BallColors = _ballCol,
            GameMode = _gameMode.ToString(),
            Angle = _angle
        };

        Container
           .Bind<SceneConfig>()
           .FromInstance(SC);
    }
}