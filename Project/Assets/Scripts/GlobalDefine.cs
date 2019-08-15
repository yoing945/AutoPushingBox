using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDefine
{
    public class FileRelateDefine
    {
        //CSV行分隔
        public const string CSV_CONFIG_ROW_SEPERATOR = "\r\n";
        //CSV基础逗号分隔符
        public const char CSV_CONFIG_BASE_SEPERATOR = ',';
        //CSV单元格内1优先级分隔符
        public const char CSV_CELL_FIRST_SEPERATOR = '|';
        //Resource目录下配置文件存放目录
        public const string CONFIG_ROOT_PATH = "Configs";
        //颜色定义配置文件名
        public const string COLOR_CONFIG_NAME = "ColorDefine";
    }
    
}
