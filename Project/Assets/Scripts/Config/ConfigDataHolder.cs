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

    public void OnInit()
    {
        ProcessColorConfig();
    }


    private void ProcessColorConfig()
    {
        var pathSeperator = Path.AltDirectorySeparatorChar;
        var path =
            GlobalDefine.FileRelateDefine.CONFIG_ROOT_PATH + pathSeperator +
            GlobalDefine.FileRelateDefine.COLOR_CONFIG_NAME;
        var datas = LoadConfig(path);

        for(int i = 1; i < datas.Length; ++i)
        {
            var cells = datas[i].Split(
                GlobalDefine.FileRelateDefine.CSV_CONFIG_BASE_SEPERATOR);
            var rgbNumStr = cells[1].Split(
                GlobalDefine.FileRelateDefine.CSV_CELL_FIRST_SEPERATOR);
            colorDict[int.Parse(cells[0])] = new Color(
                float.Parse(rgbNumStr[0]), 
                float.Parse(rgbNumStr[1]), 
                float.Parse(rgbNumStr[2]));
        }
    }


    private string[] LoadConfig(string path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        var datas = Regex.Split(textAsset.text, 
            GlobalDefine.FileRelateDefine.CSV_CONFIG_ROW_SEPERATOR);
#if UNITY_EDITOR
        if(datas == null)
        {
            Debug.LogError($"Resources/{path} 文件不存在");
        }
#endif
        return datas;
    }
}
