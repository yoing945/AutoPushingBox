using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 游戏逻辑入口
/// </summary>
public class GameMain : Singleton<GameMain>
{
    public float unitDeltaTime = 1f;

    public ArtResManager artResManager;
    public LevelManager levelManager;
    public InstructionManager instructionManager;
    public UIManager uiManager;

    public ReactiveProperty<GameModel> gameModelRP = 
        new ReactiveProperty<GameModel>(GameModel.LevelModel);

    private void Awake()
    {

        ConfigDataHolder.OnInit();
        uiManager.OnInit();

        levelManager.OnInit();
    }

}
