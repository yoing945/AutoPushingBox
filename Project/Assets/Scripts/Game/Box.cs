using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : BaseObjectOnTile
{
    public override void SetData(int level, Vector2Int logicPos, int typeInt, int objIndex)
    {
        base.SetData(level, logicPos, typeInt, objIndex);

        gameObject.name = $"Box_{index}";
    }
}
