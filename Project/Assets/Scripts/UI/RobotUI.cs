using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotUI : MonoBehaviour
{
    public Text text;

    int index = -1;
    private void Update()
    {
        if (!gameObject.activeSelf)
            return;
        if(GameMain.Instance.levelManager.allLevelsRunning)
            return;
        var level = GameMain.Instance.levelManager.GetCurrentLevel();
        if (level == null)
            return;
        var robots = level.robots;
        if (robots == null)
            return;
        if (index < 0 || index >= robots.Count)
            return;
        var robot = robots[index];
        var vec = Camera.main.WorldToScreenPoint(robot.transform.position);
        vec.y += GlobalDefine.GameDefine.UNIT_BLOCK_PIXEL_Y;
        transform.position = vec;
    }

    public void SetData(int index)
    {
        this.index = index;
        gameObject.name = $"RobotUI_{index + 1}";
        text.text = $"{index + 1}";
    }
}
