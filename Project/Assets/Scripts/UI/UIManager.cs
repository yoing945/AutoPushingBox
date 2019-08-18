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
    public InstructionElement sample;
    public Button gameModelButton;
    public GameObject bottom;

    public Button runButton;
    public Button resetButton;
    public Button levelNextButton;
    public Button levelPreButton;

    private List<InstructionElement> m_InstructionElements = 
        new List<InstructionElement>();

    public void OnInit()
    {
        m_InstructionElements.Add(sample);

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
    }

    //刷新指令条目
    private void RefreshInstrcutionElements(Level level)
    {
        var robotCount = level.robots.Count;
        if (robotCount > m_InstructionElements.Count)
        {
            for (int i = 0; i < m_InstructionElements.Count - robotCount; ++i)
                Instantiate(sample, elementsTrans);
        }
        for (int i = 0; i < robotCount; ++i)
        {
            var e = m_InstructionElements[i];
            e.SetData(i);
        }
    }

    private void RefreshButton(Level level)
    {
        levelNextButton.gameObject.SetActive(true);
        levelPreButton.gameObject.SetActive(true);

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
            text.text = "Overview";
            GameMain.Instance.gameModelRP.Value = GameModel.OverviewModel;
            bottom.SetActive(false);
        }
        else
        {
            text.text = "Level\nModel";
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
            streams.Add(e.inputField.text);
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
        var levelManager = GameMain.Instance.levelManager;
        var maxLevel = ConfigDataHolder.GetMaxLevel();
        if (maxLevel == levelManager.currentLevelRP.Value)
            levelManager.currentLevelRP.Value = ConfigDataHolder.GetMinLevel();
        else
            ++levelManager.currentLevelRP.Value;
    }
    private void OnLevelPreButtonClick()
    {
        var levelManager = GameMain.Instance.levelManager;
        var minLevel = ConfigDataHolder.GetMinLevel();
        if (minLevel == levelManager.currentLevelRP.Value)
            levelManager.currentLevelRP.Value = ConfigDataHolder.GetMaxLevel();
        else
            --levelManager.currentLevelRP.Value;
    }
}
