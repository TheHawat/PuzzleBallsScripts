using System;
using UnityEngine;

public struct ProjectConfig
{
    public KeyCode InteractKey;
    public KeyCode ResetKey;
    public int PointsForRemoved;
    public int PointsForHanging;
    public float RowHeight;
    public float BallRadius;
    public float TopBallY;
    internal float DebounceTime;
    internal float Delay;
}
