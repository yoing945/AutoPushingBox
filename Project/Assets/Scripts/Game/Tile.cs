using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : BaseBlock
{
    public TileType tileType { get; private set; }

    public void SetData(int level, Vector2Int logicPos, int typeInt)
    {
        gameObject.name = $"tile_{logicPos.x}_{logicPos.y}";

        //设置位置
        var levelData = ConfigDataHolder.levelDataDict[level];
        int matrixX = levelData[0].Length;
        int matrixY = levelData.Length;
        transform.position = PositionRelateMethods.CalcLevelBlockPosition(matrixX, matrixY, logicPos);

        this.level = level;
        this.logicPos = logicPos;
        this.tileType = (TileType)(typeInt);
        spriteRender.sprite = GameMain.Instance.artResManager.FindTileSprite(typeInt);
        animator.runtimeAnimatorController = GameMain.Instance.artResManager.FindTileAnim(typeInt);
    }

}
