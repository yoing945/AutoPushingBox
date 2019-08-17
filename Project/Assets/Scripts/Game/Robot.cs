using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Robot : BaseObjectOnTile
{
    public InstructionModule instructionModule { get; private set; }
    public MovementModule movementModule { get; private set; }

    //TODO 逻辑结构可调整
    private void Init()
    {
        if (null == instructionModule)
        {
            instructionModule = new InstructionModule(this);
        }

        if (null == movementModule)
        {
            movementModule = new MovementModule(this);
        }
        instructionModule.OnInit();
        movementModule.OnInit();
    }


    public override void SetData(int level, Vector2Int logicPos, int typeInt, int objIndex)
    {
        Init();

        base.SetData(level, logicPos, typeInt, objIndex);

        gameObject.name = $"Robot_{index}";
    }

}

public class RobotBaseModule
{
    protected Robot m_Owner;

    public virtual void OnInit(){}
}


/// <summary>
/// 指令模块
/// </summary>
public class InstructionModule: RobotBaseModule
{
    public Dictionary<int, char> instructionDict { get; private set; } =
        new Dictionary<int, char> {
            { (int)RobotState.Waiting, 'F'},
            { (int)RobotState.Up, 'W'},
            { (int)RobotState.Down, 'S'},
            { (int)RobotState.Left, 'A'},
            { (int)RobotState.Right, 'D'},
        };
    //指令流
    public string instructionStream { get; private set; } = "";
    //当前指令下标
    private ReactiveProperty<int> currentInstructionIndex = 
        new ReactiveProperty<int>(-1);

    public InstructionModule(Robot robot)
    {
        this.m_Owner = robot;
    }

    public override void OnInit()
    {
        currentInstructionIndex.Subscribe(OnCurrentInstructionIndexChanged).AddTo(m_Owner);
    }

    public void SetInstructionStream(string str)
    {
        //全部转为大写
        instructionStream = str.ToUpper();
        Debug.LogWarning($"Robot_{m_Owner.index},指令流{instructionStream}.");
    }

    public void SetCurrentInstructionIndex(int index)
    {
        m_Owner.movementModule.robotStateRP.Value = RobotState.None;
        if (index >= instructionStream.Length)
        {
            var levelManager = GameMain.Instance.levelManager;
            var level = levelManager.levels[levelManager.currentLevelIndexRP.Value];
            level.StartNewTurnDetection();
        }
        else
        {
            currentInstructionIndex.Value = index;
        }
    }

    public int GetCurrentInstructionIndex()
    {
        return currentInstructionIndex.Value;
    }

    //执行指令流
    public void ExecutionInstructionStream()
    {
        SetCurrentInstructionIndex(0);
    }


    private void OnCurrentInstructionIndexChanged(int index)
    {
        var robotState = InstructionParsing(index);
        m_Owner.movementModule.robotStateRP.Value = robotState;

    }

    //指令解析
    private RobotState InstructionParsing(int index)
    {
        if (index < 0 || index >= instructionStream.Length)
            return RobotState.None;
        char c = instructionStream[index];
        RobotState robotState;
        foreach (var pair in instructionDict)
        {
            if (pair.Value == c)
            {
                robotState = (RobotState)pair.Key;
                return robotState;
            }
        }
#if UNITY_EDITOR
            Debug.LogWarning($"Robot_{m_Owner.index},[{c}]指令被跳过, 无效指令.");
#endif
        return RobotState.None;
    }

}

/// <summary>
/// 行为模块
/// </summary>
public class MovementModule: RobotBaseModule
{
    //行为状态
    public ReactiveProperty<RobotState> robotStateRP =
        new ReactiveProperty<RobotState>(RobotState.None);

    public MovementModule(Robot robot)
    {
        this.m_Owner = robot;
    }
    public override void OnInit()
    {
        robotStateRP.Subscribe(OnRobotStateRPChanged).AddTo(m_Owner);
    }

    public void OnRobotStateRPChanged(RobotState state)
    {
        Debug.Log($"Robot_{m_Owner.index}执行指令{state}");
        if (state == RobotState.None)
            return;
        switch (state)
        {
            case RobotState.Waiting:
                WaitingUnitTime();
                break;
            case RobotState.Up:
                Up();
                break;
            case RobotState.Down:
                Down();
                break;
            case RobotState.Left:
                Left();
                break;
            case RobotState.Right:
                Right();
                break;
        }
    }

    private void WaitingUnitTime()
    {
        Observable.Timer(System.TimeSpan.FromSeconds(GameMain.Instance.unitDeltaTime)).
            Subscribe(_ => {
                var instructionModule = m_Owner.instructionModule;
                instructionModule.SetCurrentInstructionIndex(instructionModule.GetCurrentInstructionIndex() + 1);
            }).AddTo(m_Owner);
    }

    private void Up()
    {
        var levelManager = GameMain.Instance.levelManager;
        var level = levelManager.levels[levelManager.currentLevelIndexRP.Value];
        int levelMaxY = level.GetMaxY();
        if (m_Owner.logicPos.y == levelMaxY)
            return;

        var objs = new List<BaseObjectOnTile>();
        objs.Add(m_Owner);
        var x = m_Owner.logicPos.x;
        for (int y = m_Owner.logicPos.y + 1; y <= levelMaxY; ++y)
        {
            var obj = level.GetObjOnTile(x, y);
            if (obj == null)
                break;
            objs.Add(obj);
        }
        var lastObj = objs[objs.Count - 1];
        var outTile = level.GetTile(lastObj.logicPos.x, lastObj.logicPos.y + 1);

        DoMove(level, objs, outTile);
    }
    private void Down()
    {
        var levelManager = GameMain.Instance.levelManager;
        var level = levelManager.levels[levelManager.currentLevelIndexRP.Value];
        if (m_Owner.logicPos.y == 0)
            return;

        var objs = new List<BaseObjectOnTile>();
        objs.Add(m_Owner);
        var x = m_Owner.logicPos.x;
        for (int y = m_Owner.logicPos.y - 1; y >= 0; --y)
        {
            var obj = level.GetObjOnTile(x, y);
            if (obj == null)
                break;
            objs.Add(obj);
        }
        var lastObj = objs[objs.Count - 1];
        var outTile = level.GetTile(lastObj.logicPos.x, lastObj.logicPos.y - 1);

        DoMove(level, objs, outTile);
    }

    private void Right()
    {
        var levelManager = GameMain.Instance.levelManager;
        var level = levelManager.levels[levelManager.currentLevelIndexRP.Value];
        int levelMaxX = level.GetMaxX();
        if (m_Owner.logicPos.x == levelMaxX)
            return;

        var objs = new List<BaseObjectOnTile>();
        objs.Add(m_Owner);
        var y = m_Owner.logicPos.y;
        for (int x = m_Owner.logicPos.x + 1; x <= levelMaxX; ++x)
        {
            var obj = level.GetObjOnTile(x, y);
            if (obj == null)
                break;
            objs.Add(obj);
        }
        var lastObj = objs[objs.Count - 1];
        var outTile = level.GetTile(lastObj.logicPos.x + 1, lastObj.logicPos.y);

        DoMove(level, objs, outTile);
    }

    private void Left()
    {
        var levelManager = GameMain.Instance.levelManager;
        var level = levelManager.levels[levelManager.currentLevelIndexRP.Value];
        if (m_Owner.logicPos.x == 0)
            return;

        var objs = new List<BaseObjectOnTile>();
        objs.Add(m_Owner);
        var y = m_Owner.logicPos.y;
        for (int x = m_Owner.logicPos.x - 1; x >= 0; --x)
        {
            var obj = level.GetObjOnTile(x, y);
            if (obj == null)
                break;
            objs.Add(obj);
        }
        var lastObj = objs[objs.Count - 1];
        var outTile = level.GetTile(lastObj.logicPos.x - 1, lastObj.logicPos.y);

        DoMove(level, objs, outTile);
    }

    private void DoMove(Level level, List<BaseObjectOnTile> objs, Tile outTile)
    {
        Observable.Timer(System.TimeSpan.FromSeconds(GameMain.Instance.unitDeltaTime)).
            Subscribe(_ => {
                if (outTile != null && outTile.tileType != TileType.Obstacle)
                {
                    for (int i = 0; i < objs.Count - 1; ++i)
                        objs[i].MoveToTile(level.GetTile(objs[i + 1].logicPos));
                    objs[objs.Count - 1].MoveToTile(outTile);
                }
                var instructionModule = m_Owner.instructionModule;
                instructionModule.SetCurrentInstructionIndex(instructionModule.GetCurrentInstructionIndex() + 1);
            }).AddTo(m_Owner);
    }
}
