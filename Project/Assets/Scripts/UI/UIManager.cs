using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI
/// </summary>
public class UIManager : MonoBehaviour
{
    public Transform elementsTrans;
    public Transform robotUIsTrans;
    public InstructionElement sample;
    public RobotUI sampleRobotUI;
    public Button gameModelButton;
    public GameObject bottom;

    public Button runButton;
    public Button resetButton;
    public Button levelNextButton;
    public Button levelPreButton;

    private List<InstructionElement> m_InstructionElements = 
        new List<InstructionElement>();

    private List<RobotUI> m_RobotUIs =
        new List<RobotUI>();

    public void OnInit()
    {
        m_InstructionElements.Add(sample);
        //m_RobotUIs.Add(sampleRobotUI);

        gameModelButton.onClick.AddListener(OnGameModelButtonClick);
        runButton.onClick.AddListener(OnRunButtonClick);
        resetButton.onClick.AddListener(OnResetButtonClick);
        levelNextButton.onClick.AddListener(OnLevelNextButtonClick);
        levelPreButton.onClick.AddListener(OnLevelPreButtonClick);
    }

    public void RefreshUI(Level level)
    {
        RefreshInstrcutionElements(level);
        RefreshButton(level);
        RefreshRobotUI(level);
    }

    //刷新指令条目
    private void RefreshInstrcutionElements(Level level)
    {
        var robotCount = level.robots.Count;
        var capacity = m_InstructionElements.Count;
        if (robotCount > m_InstructionElements.Count)
        {
            for (int i = 0; i < robotCount - capacity; ++i)
            {
                m_InstructionElements.Add(Instantiate(sample, elementsTrans));
            }
        }
        for(int i = 0; i < m_InstructionElements.Count; ++i)
        {
            var e = m_InstructionElements[i];
            if (i < robotCount)
            {
                e.gameObject.SetActive(true);
                e.SetData(i);
            }
            else
            {
                e.gameObject.SetActive(false);
            }
        }
    }


    private void RefreshRobotUI(Level level)
    {
        var robotCount = level.robots.Count;
        var capacity = m_RobotUIs.Count;
        if (robotCount > m_RobotUIs.Count)
        {
            for (int i = 0; i < robotCount - capacity; ++i)
            {
                m_RobotUIs.Add(Instantiate(sampleRobotUI, robotUIsTrans));
            }
        }
        for (int i = 0; i < m_RobotUIs.Count; ++i)
        {
            var e = m_RobotUIs[i];
            if (i < robotCount)
            {
                e.gameObject.SetActive(true);
                e.SetData(i);
            }
            else
            {
                e.gameObject.SetActive(false);
            }
        }
    }


    private void RefreshButton(Level level)
    {
        levelNextButton.gameObject.SetActive(true);
        levelPreButton.gameObject.SetActive(false);//DEMO不需要前一关的功能

        if (!level.levelCompleted)
            levelNextButton.gameObject.SetActive(false);

        var levelManager = GameMain.Instance.levelManager;
        Level preLevel = levelManager.GetPreLevel();
        if(preLevel == null || preLevel.levelCompleted == false)
            levelPreButton.gameObject.SetActive(false);
    }

    private void OnGameModelButtonClick()
    {
        var text = gameModelButton.gameObject.GetComponentInChildren<Text>();
        if (GameMain.Instance.gameModelRP.Value == GameModel.LevelModel)
        {
            text.text = "显示";
            GameMain.Instance.gameModelRP.Value = GameModel.OverviewModel;
            bottom.SetActive(false);
        }
        else
        {
            text.text = "隐藏";
            GameMain.Instance.gameModelRP.Value = GameModel.LevelModel;
            bottom.SetActive(true);
        }
    }

    private void OnRunButtonClick()
    {
        var levelManager = GameMain.Instance.levelManager;
        var level = levelManager.GetCurrentLevel();

        var streams = new List<string>();
        foreach(var e in m_InstructionElements)
        {
            if(e.gameObject.activeSelf)
                streams.Add(e.inputField.text);
        }
        level.RunLevel(streams);
    }

    private void OnResetButtonClick()
    {
        var levelManager = GameMain.Instance.levelManager;
        var level = levelManager.GetCurrentLevel();
        level.ResetLevel();
    }
    private void OnLevelNextButtonClick()
    {
        OnResetButtonClick();
        var levelManager = GameMain.Instance.levelManager;
        var maxLevel = ConfigDataHolder.GetMaxLevel();
        if (maxLevel == levelManager.currentLevelRP.Value)
            levelManager.currentLevelRP.Value = ConfigDataHolder.GetMinLevel();
        else
            ++levelManager.currentLevelRP.Value;
    }
    private void OnLevelPreButtonClick()
    {
        OnResetButtonClick();
        var levelManager = GameMain.Instance.levelManager;
        var minLevel = ConfigDataHolder.GetMinLevel();
        if (minLevel == levelManager.currentLevelRP.Value)
            levelManager.currentLevelRP.Value = ConfigDataHolder.GetMaxLevel();
        else
            --levelManager.currentLevelRP.Value;
    }
}
