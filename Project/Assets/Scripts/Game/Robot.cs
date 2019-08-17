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
            instructionModule = new InstructionModule(this);
        if (null == movementModule)
            movementModule = new MovementModule(this);
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
}


/// <summary>
/// 指令模块
/// </summary>
public class InstructionModule: RobotBaseModule
{
    //指令流
    public string instructionStream { get; private set; } = "";
    //当前指令下标
    public ReactiveProperty<int> currentInstructionIndex = 
        new ReactiveProperty<int>(0);

    public InstructionModule(Robot robot)
    {
        this.m_Owner = robot;

        currentInstructionIndex.Subscribe(OnCurrentInstructionIndexChanged).AddTo(m_Owner);
    }

    public void SetInstructionStream(string str)
    {
        instructionStream = str;
    }

    public void ExecutionInstructionStream()
    {

    }

    private void OnCurrentInstructionIndexChanged(int index)
    {

    }
}

/// <summary>
/// 行为模块
/// </summary>
public class MovementModule: RobotBaseModule
{
    public bool isMoving { get; private set; }

    public MovementModule(Robot robot)
    {
        this.m_Owner = robot;
    }

    public void OnMove()
    {

    }
}

/// <summary>
/// 通用模块:指令解析模块
/// </summary>
public static class InstructionParsingModule
{

}