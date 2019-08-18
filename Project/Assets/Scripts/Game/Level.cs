using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Level : MonoBehaviour
{
    public Transform tilesTrans;
    public Transform robotsTrans;
    public Transform boxesTrans;

    //终点地板对应物体类型
    Dictionary<int, ObjectType> m_ToEndTileObjTypeDict =
        new Dictionary<int, ObjectType> {
            {(int)TileType.RedEnd, ObjectType.RedBox},
            {(int)TileType.YellowEnd, ObjectType.YellowBox},
        };

    public int level { get; private set; }
    private Tile[][] tileDatas;
    public List<Robot> robots { get; private set; }
    public List<Box> boxes { get; private set; }

    public bool levelRunning { get; private set; } = false;

    public ReactiveProperty<int> robotNumFinishOneTurn = 
        new ReactiveProperty<int>(0);

    private ReactiveProperty<int> finishedRoundsRP =
        new ReactiveProperty<int>(0);

    public bool levelCompleted { get; private set; } = false;
    
    private void Init()
    {
        robotNumFinishOneTurn.Subscribe(StartNewTurnDetection).AddTo(this);
        finishedRoundsRP.Subscribe(OnFinishedRoundsRPChanged).AddTo(this);
    }

    public void SetData(int level, Tile[][] tileDatas, List<Robot> robots, List<Box> boxes)
    {
        gameObject.name = $"Level_{level}";
        this.level = level;
        this.tileDatas = tileDatas;
        this.robots = robots;
        this.boxes = boxes;

        Init();

    }

    public BaseObjectOnTile GetObjOnTile(int x, int y)
    {
        foreach(var robot in robots)
        {
            if (robot.logicPos.x == x && robot.logicPos.y == y)
                return robot;
        }
        foreach (var box in boxes)
        {
            if (box.logicPos.x == x && box.logicPos.y == y)
                return box;
        }
        return null;
    }

    public Tile GetTile(int x, int y)
    {
        if ((0 <= x && x < tileDatas.Length) && (0 <= y && y < tileDatas[0].Length))
            return tileDatas[x][y];
        return null;
    }

    public Tile GetTile(Vector2Int pos)
    {
        return GetTile(pos.x, pos.y);
    }

    public int GetMaxX()
    {
        return tileDatas.Length - 1;
    }

    public int GetMaxY()
    {
        return tileDatas[0].Length - 1;
    }

    //开始新的一轮检测
    private void StartNewTurnDetection(int robotFinishedCount)
    {
        if (robotFinishedCount < robots.Count)
            return;
        robotNumFinishOneTurn.Value = 0;
        //如果正在运行所有关卡
        if(GameMain.Instance.levelManager.allLevelsRunning)
        {
            ++GameMain.Instance.levelManager.levelCountFinishedOneTurn.Value;
        }

        if(CanGainGoalDetection())
        {
            if (robotFinishedCount < GameMain.Instance.nextLevelUnlockRounds)
                ++finishedRoundsRP.Value;
            foreach (var box in boxes)
                box.ResetToInit();
        }
        else
        {
            ResetLevel();
            return;
        }
        if (Time.timeScale <5)
            Time.timeScale += 0.5f;
        foreach (var robot in robots)
            robot.instructionModule.ExecutionInstructionStream();
    }

    //检测是否能够得分
    private bool CanGainGoalDetection()
    {
        //1.boxes没到目的地 2.boxes来到错误终点 3.robots占了boxes的初始位置
        foreach(var box in boxes)
        {
            var endTile = GetTile(box.logicPos);
            var endTileTypeInt = (int)endTile.tileType;
            if (!m_ToEndTileObjTypeDict.ContainsKey(endTileTypeInt))
                return false;
            var goalObjType = m_ToEndTileObjTypeDict[endTileTypeInt];
            if (goalObjType != box.objectType)
                return false;
            endTile.PlayReciveFxAndSound();
            foreach (var robot in robots)
            {
                if (robot.logicPos == box.initLogicPos)
                    return false;
            }
        }
        return true;
    }

    //重置关卡
    public void ResetLevel()
    {
        if (!levelRunning)
            return;
        levelRunning = false;
        Debug.Log($"Level_{level} Reset!");
        Time.timeScale = 1;
        foreach(var robot in robots)
            robot.ResetToInit();
        foreach(var box in boxes)
            box.ResetToInit();

        robotNumFinishOneTurn.Value = 0;
        finishedRoundsRP.Value = 0;
    }

    //运行关卡
    public void RunLevel(List<string> streams)
    {
        if (levelRunning)
            return;
        levelRunning = true;
        Time.timeScale = 1;
        Debug.Log($"Level_{level} running!");
        SetAllRobotInstructionStream(streams);
        foreach (var robot in robots)
            robot.instructionModule.ExecutionInstructionStream();
    }

    //运行关卡:运行所有关卡时调用
    public void RunLevel()
    {
        if (levelRunning)
            return;
        levelRunning = true;
        Time.timeScale = 1;
        Debug.Log($"Level_{level} running!");
        foreach (var robot in robots)
            robot.instructionModule.ExecutionInstructionStream();
    }

    //给所有机器人设置同长指令流
    private void SetAllRobotInstructionStream(List<string> streams)
    {
        int instrcutionLength = 0;

        foreach(var stream in streams)
        {
            if (instrcutionLength < stream.Length)
                instrcutionLength = stream.Length;
        }

        if(instrcutionLength == 0)
        {
            return;
        }

        for(int robotIndex = 0; robotIndex< streams.Count; ++robotIndex)
        {
            var robot = robots[robotIndex];
            int addNum = instrcutionLength - streams[robotIndex].Length;
            char c = robot.instructionModule.instructionDict[(int)RobotState.Waiting];
            var str = "";
            for (int i = 0; i < addNum; ++i)
                str += c;
            robot.instructionModule.SetInstructionStream(streams[robotIndex] + str);
        }
    }

    private void OnFinishedRoundsRPChanged(int value)
    {
        //成功执行5轮且当前关卡是本关卡解锁下一关
        if (value < GameMain.Instance.nextLevelUnlockRounds)
            return;
        if (GameMain.Instance.levelManager.currentLevelRP.Value != level)
            return;
        levelCompleted = true;
        GameMain.Instance.uiManager.levelNextButton.gameObject.SetActive(true);
    }

}
