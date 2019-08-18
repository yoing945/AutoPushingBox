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

    public override void ResetToInit()
    {
        base.ResetToInit();
        movementModule.ResetMovement();
        instructionModule.SetCurrentInstructionIndex(-1);
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
        Debug.Log($"Robot_{m_Owner.index},指令流{instructionStream}.");
    }


    public void SetCurrentInstructionIndex(int index)
    {
        if (index >= instructionStream.Length)
        {
            var levelManager = GameMain.Instance.levelManager;
            var level = levelManager.GetCurrentLevel();
            ++level.robotNumFinishOneTurn.Value;
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
        m_Owner.movementModule.DoInstruction(robotState);
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

    //当前行为
    private System.IDisposable m_CurrentMovement;

    //行为状态
    public RobotState robotState { get; private set; }


    public MovementModule(Robot robot)
    {
        this.m_Owner = robot;
    }

    public void ResetMovement()
    {
        if (m_CurrentMovement == null)
            return;
        m_CurrentMovement.Dispose();
        robotState = RobotState.None;
    }

    public System.IDisposable DoInstruction(RobotState state)
    {
        robotState = state;
        Debug.Log($"Robot_{m_Owner.index}执行指令{state}");
        System.IDisposable movement = null;
        switch (state)
        {
            case RobotState.Waiting:
                movement = WaitingUnitTime();
                break;
            case RobotState.Up:
                movement = Up();
                break;
            case RobotState.Down:
                movement = Down();
                break;
            case RobotState.Left:
                movement = Left();
                break;
            case RobotState.Right:
                movement = Right();
                break;
        }
        m_CurrentMovement = movement;
        return movement;
    }

    private System.IDisposable WaitingUnitTime()
    {
        return Observable.Timer(System.TimeSpan.FromSeconds(GameMain.Instance.unitDeltaTime)).
            Subscribe(_ => {
                var instructionModule = m_Owner.instructionModule;
                instructionModule.SetCurrentInstructionIndex(instructionModule.GetCurrentInstructionIndex() + 1);
            }).AddTo(m_Owner);
    }

    private System.IDisposable Up()
    {
        var levelManager = GameMain.Instance.levelManager;
        var level = levelManager.GetCurrentLevel();
        int levelMaxY = level.GetMaxY();
        if (m_Owner.logicPos.y == levelMaxY)
            return DoMove(null, null, null);

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

        return DoMove(level, objs, outTile);
    }
    private System.IDisposable Down()
    {
        var levelManager = GameMain.Instance.levelManager;
        var level = levelManager.GetCurrentLevel();
        if (m_Owner.logicPos.y == 0)
            return DoMove(null, null, null);

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

        return DoMove(level, objs, outTile);
    }

    private System.IDisposable Right()
    {
        var levelManager = GameMain.Instance.levelManager;
        var level = levelManager.GetCurrentLevel();
        int levelMaxX = level.GetMaxX();
        if (m_Owner.logicPos.x == levelMaxX)
            return DoMove(null, null, null);

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

        return DoMove(level, objs, outTile);
    }

    private System.IDisposable Left()
    {
        var levelManager = GameMain.Instance.levelManager;
        var level = levelManager.GetCurrentLevel();
        if (m_Owner.logicPos.x == 0)
        {
            return DoMove(null, null, null);
        }

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

        return DoMove(level, objs, outTile);
    }

    private System.IDisposable DoMove(Level level, List<BaseObjectOnTile> objs, Tile outTile)
    {
        if (outTile != null && outTile.tileType != TileType.Obstacle)
        {
            for (int i = 0; i < objs.Count - 1; ++i)
                objs[i].MoveToTile(level.GetTile(objs[i + 1].logicPos));
            objs[objs.Count - 1].MoveToTile(outTile);
        }
        return Observable.Timer(System.TimeSpan.FromSeconds(GameMain.Instance.unitDeltaTime)).
            Subscribe(_ => {
                var instructionModule = m_Owner.instructionModule;
                instructionModule.SetCurrentInstructionIndex(instructionModule.GetCurrentInstructionIndex() + 1);
            }).AddTo(m_Owner);
    }
}
