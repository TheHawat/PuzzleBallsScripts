using System;
using UnityEngine;

public struct SceneConfig
{
    public int GridColumns;
    public int AllowedRows;
    public int FilledRows;
    public int TTL;
    public Vector2 CurrentBallPosition;
    public Vector2 NextBallPosition;
    public Color[] BallColors;
    public string GameMode;
    public float Angle;
}
