using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform tilesTrans;
    public Transform robotsTrans;
    public Transform boxesTrans;

    public int level { get; private set; }
    public Tile[][] tileDatas { get; private set; }
    public List<Robot> robots { get; private set; }
    public List<Box> boxes { get; private set; }

    //关卡是否完成
    public bool levelCompleted { get; private set; }
    

    public void SetData(int level, Tile[][] tileDatas, List<Robot> robots, List<Box> boxes)
    {
        gameObject.name = $"Level_{level}";
        this.level = level;
    }
}
