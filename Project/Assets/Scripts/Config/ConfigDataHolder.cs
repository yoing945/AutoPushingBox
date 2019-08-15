using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// 配置文件数据存储
/// </summary>
public class ConfigDataHolder
{
    public static Dictionary<int, Color> colorDict { get; private set; } = 
        new Dictionary<int, Color>();

    public static Dictionary<int, int[][]> levelDataDict { get; private set; } =
        new Dictionary<int, int[][]>();

    public static void OnInit()
    {
        LoadColorConfig();
        LoadColorConfigs();
    }


    //加载颜色配置
    private static void LoadColorConfig()
    {
        var pathSeperator = Path.AltDirectorySeparatorChar;
        var path =
            GlobalDefine.ConfigRelateDefine.CONFIG_ROOT_PATH + pathSeperator +
            GlobalDefine.ConfigRelateDefine.COLOR_CONFIG_NAME;
        var rows = LoadConfig(path);

        for(int i = 1; i < rows.Length; ++i)
        {
            var cells = rows[i].Split(
                GlobalDefine.ConfigRelateDefine.CSV_CONFIG_BASE_SEPERATOR);
            if (cells.Length < GlobalDefine.ConfigRelateDefine.COLOR_CONFIG_MAX_COL)
                continue;

            var rgbNumStr = cells[1].Split(
                GlobalDefine.ConfigRelateDefine.CSV_CELL_FIRST_SEPERATOR);
            colorDict[int.Parse(cells[0])] = new Color(
                float.Parse(rgbNumStr[0]), 
                float.Parse(rgbNumStr[1]), 
                float.Parse(rgbNumStr[2]));
        }
    }

    //加载关卡数据
    private static void LoadColorConfigs()
    {
        var pathSeperator = Path.AltDirectorySeparatorChar;
        var path =
            GlobalDefine.ConfigRelateDefine.CONFIG_ROOT_PATH + pathSeperator +
            GlobalDefine.ConfigRelateDefine.LEVEL_CONFIG_FLODER_NAME;
        var levelTextAssets = Resources.LoadAll<TextAsset>(path);
        foreach(var textAsset in levelTextAssets)
        {
            int levelIndex = 0;
            if (!int.TryParse(textAsset.name, out levelIndex))
            {
#if UNITY_EDITOR
                Debug.LogWarning($"{path}/{textAsset.name} 不是标准关卡文件的命名");
#endif
                continue;
            }
            var rows = Regex.Split(textAsset.text,
                GlobalDefine.ConfigRelateDefine.CSV_CONFIG_ROW_SEPERATOR);
            var xyStr = rows[0].Split(GlobalDefine.ConfigRelateDefine.CSV_CELL_FIRST_SEPERATOR);
            Vector2Int matrixVec2 = new Vector2Int(int.Parse(xyStr[0]), int.Parse(xyStr[1]));
            var array = new int[matrixVec2.x][];
            for(int i =1; i< 1 + matrixVec2.x; ++i)
            {
                var cells = rows[i].Split(
                    GlobalDefine.ConfigRelateDefine.CSV_CONFIG_BASE_SEPERATOR);
                var rowIntegers = new int[matrixVec2.y];
                for(int yIndex = 0; yIndex < matrixVec2.y; ++yIndex)
                {
                    rowIntegers[yIndex] = int.Parse(cells[yIndex]);
                }
                array[i - 1] = rowIntegers;
            }
            levelDataDict[levelIndex] = array;
        }
    }

    private static string[] LoadConfig(string path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        var rows = Regex.Split(textAsset.text, 
            GlobalDefine.ConfigRelateDefine.CSV_CONFIG_ROW_SEPERATOR);
#if UNITY_EDITOR
        if(rows == null)
        {
            Debug.LogError($"Resources/{path} 文件不存在");
        }
#endif
        return rows;
    }
}
