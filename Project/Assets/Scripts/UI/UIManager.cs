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

    //刷新指令条目
    public void RefreshInstructionElements(Level level)
    {
        var robotCount = level.robots.Count;
        if (robotCount > m_InstructionElements.Count)
        {
            for (int i = 0; i < m_InstructionElements.Count - robotCount; ++i)
                Instantiate(sample, elementsTrans);
        }
        for(int i = 0; i< robotCount; ++i)
        {
            var e = m_InstructionElements[i];
            e.SetData(i);
        }
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
        var level = levelManager.levels[levelManager.currentLevelIndexRP.Value];

        var streams = new List<string>();
        foreach(var e in m_InstructionElements)
            streams.Add(e.inputField.text);
        level.RunLevel(streams);
    }

    private void OnResetButtonClick()
    {

    }
    private void OnLevelNextButtonClick()
    {

    }
    private void OnLevelPreButtonClick()
    {

    }
}
