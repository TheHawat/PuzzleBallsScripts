using UnityEngine;
using Zenject;

public class IndicatorBehaviour : MonoBehaviour
{
    [Inject] private SceneConfig _sceneConfig;
    [Inject] private ProjectConfig _projectConfig;
    public GameObject Target;
    public GameObject Indicator;
    public GameObject Left;
    public GameObject Right;
    public BoxCollider2D LeftBox;
    public BoxCollider2D RightBox;
    private Vector3 _zAxis;
    private readonly KeyCode _rKey = KeyCode.LeftArrow;
    private readonly KeyCode _uKey = KeyCode.UpArrow;
    private readonly KeyCode _lKey = KeyCode.RightArrow;
    private float _nextTick;
    private float _minAngle;
    private float _maxAngle;
    public void Start() {
        _zAxis = new Vector3(_sceneConfig.CurrentBallPosition.x, _sceneConfig.CurrentBallPosition.y, -10000000);
        _minAngle = -_sceneConfig.Angle;
        _maxAngle = _sceneConfig.Angle;
    }
    public void Update() {
        int Angle = (int)Indicator.transform.eulerAngles.z;
        if (Angle > 180) Angle -= 360;
        UpdateTick(Angle);
    }
    private void UpdateTick(int angle) {
        bool StartLeft = false;
        bool GoLeft = false;
        bool StartRight = false;
        bool GoRight = false;
        Vector3 TouchPos = new();
        if (Input.touchCount > 0) {
            TouchPos = Input.touches[0].position;
            Ray ray = Camera.main.ScreenPointToRay(TouchPos);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            switch (Input.touches[0].phase) {
                case TouchPhase.Began:
                    if (hit.collider == LeftBox) {
                        StartRight = true;
                    }
                    if (hit.collider == RightBox) {
                        StartLeft = true;
                    }
                    break;
                case TouchPhase.Moved: case TouchPhase.Stationary:
                    if (hit.collider == LeftBox) {
                        GoRight = true;
                    }
                    if (hit.collider == RightBox) {
                        GoLeft = true;
                    }
                    break;
            }
        }
        StartLeft = StartLeft || Input.GetKeyDown(_lKey);
        StartRight = StartRight || Input.GetKeyDown(_rKey);
        GoLeft = GoLeft || Input.GetKey(_lKey);
        GoRight = GoRight || Input.GetKey(_rKey);

        if (StartLeft && angle > _minAngle) {
            _nextTick = UpdateNextTick(1, _projectConfig.Delay);
        }
        if (GoLeft && angle > _minAngle && Time.time > _nextTick) {
            _nextTick = UpdateNextTick(1, _projectConfig.DebounceTime);
        }

        if (StartRight && angle < _maxAngle) {
            _nextTick = UpdateNextTick(-1, _projectConfig.Delay);
        }
        if (GoRight && angle < _maxAngle && Time.time > _nextTick) {
            _nextTick = UpdateNextTick(-1, _projectConfig.DebounceTime);
        }






        if (Input.GetKeyDown(_uKey) && angle > 0) {
            _nextTick = UpdateNextTick(1, _projectConfig.Delay);
        }
        if (Input.GetKey(_uKey) && angle > 0 && Time.time > _nextTick) {
            _nextTick = UpdateNextTick(1, _projectConfig.DebounceTime);
        }

        if (Input.GetKeyDown(_uKey) && angle < 0) {
            _nextTick = UpdateNextTick(-1, _projectConfig.Delay);
        }
        if (Input.GetKey(_uKey) && angle < 0 && Time.time > _nextTick) {
            _nextTick = UpdateNextTick(-1, _projectConfig.DebounceTime);
        }
    }


    private float UpdateNextTick(int sign, float delay) {
        Indicator.transform.RotateAround(_sceneConfig.CurrentBallPosition, _zAxis, sign);
        Left.transform.RotateAround(Left.transform.position, _zAxis, -sign);
        Right.transform.RotateAround(Right.transform.position, _zAxis, -sign);
        return Time.time + delay;
    }
    public Vector2 CalcStartingVelocity() {
        float Degrees = Indicator.transform.eulerAngles.z;
        float Radians = (Degrees + 90.0f) * 3.1415f / 180.0f;
        float _multiplier = 10.0f;
        float _startingX = _multiplier * Mathf.Cos(Radians);
        float _startingY = _multiplier * Mathf.Sin(Radians);
        return new Vector2(_startingX, _startingY);
    }
}
