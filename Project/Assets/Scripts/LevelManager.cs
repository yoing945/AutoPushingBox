using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 关卡管理器
/// </summary>
public class LevelManager : MonoBehaviour
{
    public Level sampleLevel;
    public Tile sampleTile;
    public Robot sampleRobot;
    public Box sampleBox;

    public void OnInit()
    {
        GenerateLevel(1);
    }

    //产生关卡
    public void GenerateLevel(int level)
    {
        var newLevel = Instantiate(sampleLevel, transform);

        var levelData = ConfigDataHolder.levelDataDict[level];
        int matrixX = levelData[0].Length;
        int matrixY = levelData.Length;

        var tileDatas = new Tile[matrixX][];
        var robots = new List<Robot>();
        var boxes = new List<Box>();

        for(int x = 0; x < matrixX; ++x )
        {
            var colxCells = new Tile[matrixY];
            for(int y = 0; y < matrixY; ++y)
            {
                int blockValue = PositionRelateMethods.GetLevelBlockValue(level, x, y);

                //创建地块
                var newTile = Instantiate(sampleTile, newLevel.tilesTrans);
                int tileTypeInt = blockValue % 10;
                newTile.SetData(level, new Vector2Int(x, y), tileTypeInt);
                colxCells[y] = newTile;

                int objTypeInt = blockValue / 10;
                if (objTypeInt == 0)
                    continue;
                var objectType = (ObjectType)objTypeInt;
                if (objectType == ObjectType.Robot)
                {
                    //创建机器人
                    var newObj = Instantiate(sampleRobot, newLevel.robotsTrans);
                    newObj.SetData(level, new Vector2Int(x, y), objTypeInt, robots.Count);
                    robots.Add(newObj);
                }
                else
                {
                    //创建箱子
                    var newObj = Instantiate(sampleBox, newLevel.boxesTrans);
                    newObj.SetData(level, new Vector2Int(x, y), objTypeInt, boxes.Count);
                    boxes.Add(newObj);
                }
                
            }
            tileDatas[x] = colxCells;
        }

        newLevel.SetData(level, tileDatas, robots, boxes);
    }

}
