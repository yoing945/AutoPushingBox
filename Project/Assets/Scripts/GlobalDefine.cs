using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDefine
{
    public class ConfigRelateDefine
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
        //颜色定义配置文件最大列数
        public const int COLOR_CONFIG_MAX_COL = 2;

        //关卡配置文件目录名
        public const string LEVEL_CONFIG_FLODER_NAME = "Levels";
    }

    public class GameDefine
    {
        //每个像素边长对应unity中的0.01个单位;
        public const float UNIT_PER_PIXEL = 0.01f;

        public const int UNIT_BLOCK_PIXEL_X = 32;
        public const int UNIT_BLOCK_PIXEL_Y = 32;
        public const int UNIT_BLOCK_SPACING_X = 2;
        public const int UNIT_BLOCK_SPACING_Y = 2;
    }

}

public enum TileType
{
    Normal = 0,
    Obstacle,
    YellowEnd,
    RedEnd,
    Red
}

public enum ObjectType
{
    None = 0,
    Robot,
    YellowBox,
    RedBox
}

public enum RobotState
{
    Stop = -1,
    None = 0,
    Waiting,
    Up,
    Down,
    Left,
    Right
}

public enum GameModel
{
    //单关卡模式
    LevelModel,
    //总览模式
    OverviewModel
}