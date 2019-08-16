using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockData
{
    public TileType tileType { get; private set; }
    public ObjectType objectType { get; private set; }

    public BlockData (int tileTypeInt, int objectTypeInt)
    {
        tileType = (TileType)tileTypeInt;
        objectType = (ObjectType)objectTypeInt;
    }
}

public enum TileType
{
    Normal = 0,
    Obstacle,
    YellowEnd,
    RedEnd,
    Red
}

public enum ObjectType
{
    None = 0,
    Robot,
    YellowBox,
    RedBox
}
