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
        inputField.contentType = InputField.ContentType.Alphanumeric;
    }

    public void SetData(int robotIndex)
    {
        this.robotIndex = robotIndex;
        normalText.text = $"{robotIndex + 1}号机器人";
        inputField.text = "";
    }

}
