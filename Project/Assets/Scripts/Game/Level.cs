using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Tile sampleTile;
    public ObjectOnTile sameleObjectOnTile;
    public Transform tilesTrans;
    public Transform objectsOnTileTrans;

    public Tile[][] tileDatas { get; set; }
    public List<ObjectOnTile> robots { get; set; }
    public List<ObjectOnTile> boxes { get; set; }
}
