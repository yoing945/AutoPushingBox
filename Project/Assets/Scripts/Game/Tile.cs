using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : BaseBlock
{
    public TileType tileType { get; private set; }

    public override void SetData(int level, Vector2Int logicPos)
    {
        //设置位置
        var levelData = ConfigDataHolder.levelDataDict[level];
        int matrixX = levelData[0].Length;
        int matrixY = levelData.Length;
        transform.position = PositionRelateMethods.CalcLevelBlockPosition(matrixX, matrixY, logicPos);

        this.level = level;
        this.logicPos = logicPos;
        int tileValue = PositionRelateMethods.GetLevelBlockValue(level, logicPos);
        //右数第一位为地块类型
        int tileTypeInt = tileValue % 10;
        tileType = (TileType)(tileTypeInt);
        spriteRender.sprite = GameMain.Instance.artResManager.FindTileSprite(tileTypeInt);
        animator.runtimeAnimatorController = GameMain.Instance.artResManager.FindTileAnim(tileTypeInt);
    }

}
