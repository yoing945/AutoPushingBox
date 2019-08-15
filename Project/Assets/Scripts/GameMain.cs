using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏逻辑入口
/// </summary>
public class GameMain : Singleton<GameMain>
{
    public LevelGenerator levelGenerator;

    private void Awake()
    {
        ConfigDataHolder.OnInit();

    }

}
