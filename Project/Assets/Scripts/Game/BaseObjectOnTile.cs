using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjectOnTile : BaseBlock
{
    public int index { get; protected set; }
    public ObjectType objectType { get; protected set; }

    private void SetData(int level, Vector2Int logicPos, int typeInt)
    {
        var objectType = (ObjectType)(typeInt);

        //设置位置
        var levelData = ConfigDataHolder.levelDataDict[level];
        int matrixX = levelData[0].Length;
        int matrixY = levelData.Length;
        transform.position = PositionRelateMethods.CalcLevelBlockPosition(matrixX, matrixY, logicPos);

        this.level = level;
        this.logicPos = logicPos;
        this.objectType = objectType;
        spriteRender.sprite = GameMain.Instance.artResManager.FindObjectOnTileSprite(typeInt);
        animator.runtimeAnimatorController = GameMain.Instance.artResManager.FindObjectOnTileAnim(typeInt);
    }

    public virtual void SetData(int level, Vector2Int logicPos, int typeInt, int index)
    {
        SetData(level, logicPos, typeInt);
        this.index = index;
    }

}
