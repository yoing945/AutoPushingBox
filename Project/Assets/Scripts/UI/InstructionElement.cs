using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 指令输入元素
/// </summary>
public class InstructionElement : MonoBehaviour
{
    
    public Text normalText;
    public InputField inputField;

    public int robotIndex { get; private set; }

    private void Awake()
    {
        inputField.onEndEdit.AddListener(OnEndEdit);
    }

    public void SetData(int robotIndex)
    {
        this.robotIndex = robotIndex;
        inputField.text = "";
    }

    private void OnEndEdit(string str)
    {
        var levelManager = GameMain.Instance.levelManager;
        var robot = levelManager.levels[levelManager.currentLevelRP.Value].robots[robotIndex];
        robot.instructionModule.SetInstructionStream(str);
    }

}
