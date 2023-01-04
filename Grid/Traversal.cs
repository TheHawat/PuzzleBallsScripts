using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

public class Traversal : MonoBehaviour
{
    public BGrid _grid;
    [Inject] private IAudioManager _speaker;
    public int CheckForRemoval(int X, int Y) {
        List<(int, int)> ToBeDestroyed = new();
        Color ColorToCheck = _grid.Grid[X][Y].GetComponent<SpriteRenderer>().color;
        TraverseGraphColors((X, Y), ref ToBeDestroyed, ColorToCheck);
        if (ToBeDestroyed.Count < 3) return 0;
        KillBalls(ToBeDestroyed);
        return ToBeDestroyed.Count;
    }
    public int DropHanging() {
        int Hanging = 0;
        List<(int, int)> ToBeSaved = new();
        bool Ceiling = false;
        for (int i = 0; i < _grid.Grid.Count; i++) {
            for (int j = 0; j < _grid.Grid[i].Length; j++) {
                if (_grid.Grid[i][j] == null) continue;
                if (!ToBeSaved.Contains((i, j))) TraverseGraphCeiling((i, j), ref ToBeSaved, ref Ceiling);
                if (!Ceiling) {
                    DropOneBall(i, j);
                    Hanging++;
                }
                Ceiling = false;
                ToBeSaved.Clear();
            }
        }
        return Hanging;
    }
    private void TraverseGraphColors((int, int) currentNode, ref List<(int, int)> killList, Color currCol) {
        (int X, int Y) = currentNode;
        if (_grid.Grid[X][Y].GetComponent<SpriteRenderer>().color != currCol) return;
        killList.Add(currentNode);
        List<(int, int)> _neighbours = _grid.NeighboursOfCoords(X, Y);
        foreach (var Node in _neighbours) {
            if (!killList.Contains(Node)) {
                TraverseGraphColors(Node, ref killList, currCol);
            }
        }
    }
    private void TraverseGraphCeiling((int, int) currentNode, ref List<(int, int)> safeList, ref bool ceiling) {
        (int X, int Y) = currentNode;
        if (ceiling) return;
        if (X == 0) { ceiling = true; return; }
        safeList.Add(currentNode);
        List<(int, int)> _neighbours = _grid.NeighboursOfCoords(X, Y);
        foreach (var Node in _neighbours) {
            if (!safeList.Contains(Node)) TraverseGraphCeiling(Node, ref safeList, ref ceiling);
        }
    }
    internal void KillBalls(List<(int, int)> ballsToKill) {
        foreach ((int, int) ball in ballsToKill) {
            KillOneBall(ball.Item1, ball.Item2);
        }
    }
    private void KillOneBall(int X, int Y) {
        GameObject ball = _grid.Grid[X][Y];
        MonoBehaviour.Destroy(ball);
        _grid.RemoveBall(X, Y);
        _speaker.PlayTheSoundOf("Pop");
    }
    private async void DropOneBall(int i, int j) {
        GameObject ball = _grid.Grid[i][j];
        _grid.RemoveBall(i, j);
        BallBehaviour bo = ball.GetComponent<BallBehaviour>();
        bo.Drop();
        await UniTask.Delay(80);
        _speaker.PlayTheSoundOf("Swing");
        await UniTask.Delay(4800);
        MonoBehaviour.Destroy(ball);
    }
}
