using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BGrid : MonoBehaviour
{
    private float _leftBallX;
    private List<GameObject[]> _grid;
    public List<GameObject[]> Grid => _grid;
    private List<Vector2[]> _gridPositions;
    public List<Vector2[]> GridPositions => _gridPositions;
    [Inject] private ProjectConfig _projectConfig;
    [Inject] private SceneConfig _sceneConfig;
    public void Start() {
        _leftBallX = -((float)_sceneConfig.GridColumns / 2) + 0.5f;
        _grid = new();
        _gridPositions = new();
        CreateNewRowBottom();
    }
    public GameObject CoordsToGO((int X, int Y) ball) => _grid[ball.X][ball.Y];
    public void AddBall(int row, int column, GameObject go) {
        if (_grid[row][column] != null) throw new NotSupportedException();
        _grid[row][column] = go;
        if (row == _grid.Count - 1) CreateNewRowBottom();
    }
    public int LowestRow() {
        int lowestRow = -1;
        for (int i = 0; i < _grid.Count; i++) {
            for (int j = 0; j < _grid[i].Length; j++) {
                if (_grid[i][j] != null) lowestRow = i;
            }
        }
        return lowestRow;
    }
    public void RemoveBall(int row, int column) => _grid[row][column] = null;
    public List<GameObject> NeighboursOf(int row, int column) {
        List<(int, int)> coords = NeighboursOfCoords(row, column);
        List<GameObject> l = coords.ConvertAll(CoordsToGO);
        return l;
    }
    public List<(int, int)> NeighboursOfCoords(int row, int column) {
        List<(int, int)> l = new();
        int NextY = column;
        if (_grid[row].Length == _sceneConfig.GridColumns) NextY--;
        List<(int X, int Y)> BallsToCheck = GenerateAdjBallList((row, column), NextY);
        foreach (var ball in BallsToCheck) {
            if (InBounds(ball) && _grid[ball.X][ball.Y] != null) l.Add(ball);
        }
        return l;
    }
    public (int, int) FindNearest(Vector2 position) {
        float MinimumDistance = float.MaxValue;
        int MinimalI = 0, MinimalJ = 0;
        for (int i = 0; i < _grid.Count; i++) {
            int RowColums = _grid[i].Length;
            for (int j = 0; j < RowColums; j++) {
                if (_grid[i][j] != null) continue;
                float Distance = Vector2.Distance(GridPositions[i][j], position);
                if (Distance < MinimumDistance) {
                    MinimumDistance = Distance;
                    MinimalI = i;
                    MinimalJ = j;
                }
            }
        }
        return (MinimalI, MinimalJ);
    }
    private bool InBounds((int X, int Y) ball) {
        return ball.X >= 0 && ball.X < _grid.Count && ball.Y >= 0 && ball.Y < _grid[ball.X].Length;
    }
    private List<(int, int)> GenerateAdjBallList((int X, int Y) coords, int nextY) {
        return new(){
            (coords.X -1, nextY),
            (coords.X -1, nextY + 1),
            (coords.X +1, nextY),
            (coords.X +1, nextY + 1),
            (coords.X, coords.Y + 1),
            (coords.X, coords.Y -1) };
    }
    public void CreateNewRowTop() {
        LowerBalls();
        bool isLongerColumn = (Grid[0].Length == (_sceneConfig.GridColumns - 1));
        ExpandGrids(0, isLongerColumn);
    }
    public void CreateNewRowBottom() {
        int newRowIndex = Grid.Count;
        bool isLongerColumn = (newRowIndex == 0) || (Grid[^1].Length == _sceneConfig.GridColumns - 1);
        ExpandGrids(newRowIndex, isLongerColumn);
    }
    private void ExpandGrids(int row, bool isLongerColumn) {
        int RowColumns = (isLongerColumn) ? _sceneConfig.GridColumns : _sceneConfig.GridColumns - 1; ;
        float RowCorrection = (isLongerColumn) ? 0.0f : 0.5f; ;
        _grid.Insert(row, new GameObject[RowColumns]);
        _gridPositions.Insert(row, new Vector2[RowColumns]);
        for (int j = 0; j < RowColumns; j++) {
            float X = _leftBallX + (j * 2 * _projectConfig.BallRadius) + RowCorrection;
            float Y = _projectConfig.TopBallY - (row * _projectConfig.RowHeight);
            _gridPositions[row][j] = new Vector2(X, Y);
        }
    }
    public void TrimEmptyRows() {
        int lowestRow = LowestRow();
        _grid.RemoveRange(lowestRow + 2, _grid.Count - lowestRow - 2);
        _gridPositions.RemoveRange(lowestRow + 2, _gridPositions.Count - lowestRow - 2);
    }
    internal void InsertLine(int v, GameObject[] line) {
        _grid.Insert(v, line);
    }
    public void LowerBalls() {
        for (int i = _gridPositions.Count - 1; i >= 0; i--) {
            for (int j = _gridPositions[i].Length - 1; j >=0; j--) {
                Vector2 Position = _gridPositions[i][j];
                Position.y -= _projectConfig.RowHeight;
                _gridPositions[i][j] = Position;
                if(_grid[i][j] != null) _grid[i][j].transform.position = _gridPositions[i][j];
            }
        }
    }
    public void LowerBallsPrebuilt() {
        _projectConfig.TopBallY -= _projectConfig.RowHeight;
        LowerBalls();
    }
}
