using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Level : MonoBehaviour
{
    public Transform tilesTrans;
    public Transform robotsTrans;
    public Transform boxesTrans;

    private int m_RobotNumFinishOneTurn = 0;
    public int levelNum { get; private set; }
    private Tile[][] tileDatas;
    public List<Robot> robots { get; private set; }
    public List<Box> boxes { get; private set; }

    public bool levelRunning { get; private set; } = false;

    private ReactiveProperty<int> finishedRoundsRP =
        new ReactiveProperty<int>(0);
    

    public void SetData(int level, Tile[][] tileDatas, List<Robot> robots, List<Box> boxes)
    {
        gameObject.name = $"Level_{level}";
        this.levelNum = level;
        this.tileDatas = tileDatas;
        this.robots = robots;
        this.boxes = boxes;
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

    public void StartNewTurnDetection()
    {
        ++m_RobotNumFinishOneTurn;
        if (m_RobotNumFinishOneTurn < robots.Count)
            return;
        m_RobotNumFinishOneTurn = 0;

        if(finishedRoundsRP.Value < GameMain.Instance.nextLevelUnlockRounds)
            ++finishedRoundsRP.Value;
        foreach (var robot in robots)
            robot.instructionModule.ExecutionInstructionStream();
    }

    public void RunLevel(List<string> streams)
    {
        if (levelRunning)
            return;
        levelRunning = true;
        Debug.Log($"Level_{levelNum} running!");
        SetAllRobotInstructionStream(streams);
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
        if (value < GameMain.Instance.nextLevelUnlockRounds)
            return;
        //TODO 解锁下一关
    }
}
