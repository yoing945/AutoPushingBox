using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏逻辑入口
/// </summary>
public class GameMain : Singleton<GameMain>
{
    public float unitDeltaTime = 1f;

    public GameModel gameModel { get; private set; } = 
        GameModel.LevelModel;

    public ArtResManager artResManager;
    public LevelManager levelManager;
    public InstructionManager instructionManager;

    private void Awake()
    {
        ConfigDataHolder.OnInit();
        levelManager.OnInit();
    }

}
