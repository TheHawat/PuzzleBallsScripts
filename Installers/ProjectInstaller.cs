using Zenject;
using UnityEngine;
using System.Collections;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private KeyCode _interactKey = KeyCode.Space;
    [SerializeField] private KeyCode _resetKey = KeyCode.R;
    private int _pointsForRemoved = 10;
    private int _pointsForHanging = 5;
    private float _rowHeight = 0.83f;
    private float _radius = 0.5f;
    private float _topBallY = 3.94f;
    private float _debounceTime = 0.01f;
    private float _delay = 0.1f;
    public override void InstallBindings() {
        ProjectConfig PC = new ProjectConfig {
            ResetKey = _resetKey,
            InteractKey = _interactKey,
            PointsForRemoved = _pointsForRemoved,
            PointsForHanging = _pointsForHanging,
            RowHeight = _rowHeight,
            BallRadius = _radius,
            TopBallY = _topBallY,
            DebounceTime = _debounceTime,
            Delay = _delay
        };

        Container
           .Bind<ProjectConfig>()
           .FromInstance(PC);

        // TODO: Ugly
        SceneLoader SceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();

        Container
           .Bind<SceneLoader>()
           .FromInstance(SceneLoader)
           .AsSingle();

    }
}