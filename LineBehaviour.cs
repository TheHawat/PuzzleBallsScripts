using System;
using UnityEngine;
using Zenject;

public class LineBehaviour : MonoBehaviour
{
    [Inject] private SceneConfig _sceneConfig;
    [Inject] private ProjectConfig _projectConfig;
    void Start() {
        RepositionRope();
    }
    private void RepositionRope() {
        float StartingPosX = -((float)_sceneConfig.GridColumns / 2);
        float StartingPosY = this.transform.position.y;
        if(this.CompareTag("Rope")) StartingPosY = _projectConfig.TopBallY - _sceneConfig.AllowedRows * _projectConfig.RowHeight;
        this.transform.position = new Vector2(StartingPosX, StartingPosY);
        int AmountToEnable = CalculateSegmentAmount();
        EnableSegments(AmountToEnable);
    }
    private void EnableSegments(int howMany) {
        SpriteRenderer[] AllSegments = this.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < howMany; i++) {
            AllSegments[i].enabled = true;
        }
    }
    private int CalculateSegmentAmount() {
        int Result = 0;
        float Width = 0;
        float SegmentWidth = 1.484f; // TODO: Change this to get object width.
        while (Width < _sceneConfig.GridColumns) {
            Result++;
            Width += SegmentWidth;
        }
        return Result;
    }

    public void LowerLine() {
        if (this.CompareTag("Rope")) return;
        Vector2 Position = this.transform.position;
        Position.y -= _projectConfig.RowHeight;
        this.transform.position = Position;
    }
}
