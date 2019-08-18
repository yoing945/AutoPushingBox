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
            endTile.PlayFX();
        }
        
    }

    public override void ResetToInit()
    {
        base.ResetToInit();
        this.objectType = this.initObjType;
    }

}
