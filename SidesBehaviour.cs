using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SidesBehaviour : MonoBehaviour
{
    [Inject] private SceneConfig _sceneConfig;
    [Inject] private ProjectConfig _projectConfig;
    void Start() {
        RepositionWalls();
    }
    private void RepositionWalls() {
        float WallPosition = (0.65f + ((float)_sceneConfig.GridColumns / 2));
        SpriteRenderer[] AllSegments = this.GetComponentsInChildren<SpriteRenderer>();
        Vector2 OldPosition = AllSegments[0].transform.position;
        float StartingPosY = _projectConfig.TopBallY - _sceneConfig.AllowedRows * _projectConfig.RowHeight;
        AllSegments[0].transform.position = new Vector2(-WallPosition, OldPosition.y);
        AllSegments[1].transform.position = new Vector2(WallPosition, OldPosition.y);
        AllSegments[2].transform.position = new Vector2((-WallPosition + 0.62f), StartingPosY);
        AllSegments[3].transform.position = new Vector2((WallPosition - 0.62f), StartingPosY);
    }
}