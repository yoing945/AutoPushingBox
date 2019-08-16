using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 关卡管理器
/// </summary>
public class LevelManager : MonoBehaviour
{
    public Level sampleLevel;

    public void OnInit()
    {

    }

    //产生关卡
    public void GenerateLevel(int level)
    {
        var newLevel = Instantiate(sampleLevel);
        newLevel.gameObject.name = $"Level_{level}";

        var levelData = ConfigDataHolder.levelDataDict[level];
        int matrixX = levelData[0].Length;
        int matrixY = levelData.Length;

        for(int x = 0; x < matrixX; ++x )
        {
            for(int y = 0; y < matrixY; ++y)
            {
                var newTile = Instantiate(newLevel.sampleTile, newLevel.tilesTrans);
                newTile.SetData(level, new Vector2Int(x, y));
            }
        }
    }

}
