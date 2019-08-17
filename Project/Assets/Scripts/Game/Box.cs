using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : BaseObjectOnTile
{
    public ObjectType initObjType { get; private set; }

    //染色地板对应物体类型
    Dictionary<int, ObjectType> m_ChangeObjTypeDict =
        new Dictionary<int, ObjectType> {
            {(int)TileType.Red, ObjectType.RedBox},
        };

    //终点地板对应物体类型
    Dictionary<int, ObjectType> m_ToEndTileObjTypeDict =
        new Dictionary<int, ObjectType> {
            {(int)TileType.RedEnd, ObjectType.RedBox},
            {(int)TileType.YellowEnd, ObjectType.YellowBox},
        };

    public override void SetData(int level, Vector2Int logicPos, int typeInt, int objIndex)
    {
        base.SetData(level, logicPos, typeInt, objIndex);
        this.initObjType = (ObjectType)typeInt;
        gameObject.name = $"Box_{index}";
    }

    protected override void EndMove(Tile endTile)
    {
        base.EndMove(endTile);

        var tileTypeInt = (int)endTile.tileType;
        if (m_ChangeObjTypeDict.ContainsKey(tileTypeInt))
        {
            this.objectType = m_ChangeObjTypeDict[tileTypeInt];
        }
        else if(m_ToEndTileObjTypeDict.ContainsKey(tileTypeInt))
        {
            var goalObjType = m_ToEndTileObjTypeDict[tileTypeInt];
            if (goalObjType == this.objectType)
                GainGoal();
            else
                ToWrongEnd();
        }

    }

    //得分
    private void GainGoal()
    {
        Debug.Log($"得分！");
        base.ResetToInit();
    }

    //箱子到达错误终点
    private void ToWrongEnd()
    {
        Debug.Log($"箱子到达错误终点, 重置");
    }

    public override void ResetToInit()
    {
        base.ResetToInit();
        this.objectType = this.initObjType;
    }

}
