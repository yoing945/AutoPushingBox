using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOnTile : BaseBlock
{
    public ObjectType objectData { get; private set; }

    public override void SetData(int level, Vector2Int logPos)
    {
    }
}
