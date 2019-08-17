using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : BaseObjectOnTile
{
    public bool isMoving { get; private set; }
    public InstructionModel instructionModel { get; private set; }
    public MovementModel movementModel { get; private set; }

    //TODO 逻辑结构可调整
    private void Init()
    {
        if (null == instructionModel)
            instructionModel = new InstructionModel(this);
        if (null == movementModel)
            movementModel = new MovementModel(this);
    }


    public override void SetData(int level, Vector2Int logicPos, int typeInt, int objIndex)
    {
        Init();

        base.SetData(level, logicPos, typeInt, objIndex);

        gameObject.name = $"Robot_{index}";
    }

}

public class RobotBaseModel
{
    protected Robot m_Owner;

}


/// <summary>
/// 指令模块
/// </summary>
public class InstructionModel: RobotBaseModel
{
    //指令流
    public string instruction { get; private set; }
    //当前指令下标
    public int currentIndex { get; private set; }

    public InstructionModel(Robot robot)
    {
        this.m_Owner = robot;
    }

    public void SetInstruction(string str)
    {
        instruction = str;
    }
}

/// <summary>
/// 行为模块
/// </summary>
public class MovementModel: RobotBaseModel
{
    public MovementModel(Robot robot)
    {
        this.m_Owner = robot;
    }
}