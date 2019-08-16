using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PositionRelateMethods
{
    //获取配置表关卡矩阵中元素值
    public static int GetLevelBlockValue(int level, Vector2Int logicPos)
    {
        var array = ConfigDataHolder.levelDataDict[level];
        var row = array.Length - logicPos.y;
        var col = logicPos.x;
        return array[row][col];
    }

    //左下角作为原点
    public static Vector2 CalcLevelOrigionPos(int matrixX, int matrixY)
    {
        var x = -0.5f * (matrixX - 1) * (GlobalDefine.GameDefine.UNIT_BLOCK_PIXEL_X +
            GlobalDefine.GameDefine.UNIT_BLOCK_SPACING_X);
        var y = -0.5f * (matrixY - 1) * (GlobalDefine.GameDefine.UNIT_BLOCK_PIXEL_Y +
            GlobalDefine.GameDefine.UNIT_BLOCK_SPACING_Y);
        return new Vector2(x, y);
    }

    public static Vector2 CalcLevelBlockPosition(int matrixX, int matrixY, Vector2Int pos)
    {
        return CalcLevelBlockPosition(matrixX, matrixY, pos.x, pos.y);
    }

    public static Vector2 CalcLevelBlockPosition(int matrixX, int matrixY, int x, int y)
    {
        var origionPos = CalcLevelOrigionPos(matrixX, matrixY);
        return CalcLevelBlockPosition(origionPos, x, y);
    }

    //TODO 可优化的
    public static Vector2 CalcLevelBlockPosition(Vector2 origionPos, int x, int y)
    {
        return new Vector2(
            origionPos.x + 1f * x * (GlobalDefine.GameDefine.UNIT_BLOCK_PIXEL_X +
                GlobalDefine.GameDefine.UNIT_BLOCK_SPACING_X),
            origionPos.y + 1f * y * (GlobalDefine.GameDefine.UNIT_BLOCK_PIXEL_Y +
                GlobalDefine.GameDefine.UNIT_BLOCK_SPACING_Y)
            );
    }
}
