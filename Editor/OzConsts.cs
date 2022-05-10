using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OzConsts
{
    // GENERAL
    public const string MENU_ITEM = "Tools/OrganiZer";
    public const string MAIN_POS = "position";
    public const string EMPTY = "";
    public const string ZERO = "0";
    public const string OK_BTN = "OK";
    public const string CHAR_FILTER = @"[^0-9]";
    public const string CHAR_LIMIT_FORMAT = "{0:0} / {1:0}";
    
    public const int BASE_SPACE = 10;
    public const int MAIN_WIDTH = 360;
    public const int MAIN_HEIGHT = 380;
    
    // TASK BOARD
    public const string NEW_COL_FORMAT = " {0:s}\n{1:s}";
    public const string PLUS = "+";

    public const int TASK_WDW_WIDTH = 400;
    public const int TASK_WDW_HEIGHT = 500;
    public const int COL_WDW_WIDTH = 150;
    public const int COL_WDW_HEIGHT = 160;
    public const int MAX_TASK_COLUMNS = 10;
    public const int MAX_TASKS = 10;
    public const int TASK_WIDTH = 180;
    public const int TASK_HEIGHT = 60;
    public const int COLUMN_WIDTH = TASK_WIDTH + BASE_SPACE * 2;
    public const int COLUMN_HEIGHT = TASK_HEIGHT + BASE_SPACE * 2;
    public const int NAME_LIMIT = 25;
    public const int DESCR_LIMIT = 300;
    public const int STEPS_LIMIT = 10;
    public const int OBS_LIMIT = 500;

    // POMODORO
    public const string MODE_LBL_FORMAT = "\n{0:s}: ";
    public const string TIMER_FORMAT = "{0:00}:{1:00}:{2:00}";
    public const string PREFAB_PATH = "Packages/com.zekelionhart.organizer/Resources/OzPomodoroObject.prefab"/*  Assets/Editor/Pomodoro/OzPomodoroAlert.prefab*/;

    public const int POMO_CHAR_LIMIT = 2;
    public const int POMO_WDW_POS_Y = 40;
    public const int POMO_WDW_WIDTH = 125;
    public const int POMO_WDW_HEIGHT = 135;

    // NOTES-PIPELINES
    public const string NOTE_LIMIT_FORMAT = "{0:s}s: {1:0} / {2:0}";

    public const int MAX_NOTE_COLUMNS = 10;
    public const int TITLE_LIMIT = 40;
    public const int BODY_LIMIT = 500;
    public const int NOTE_WDW_WIDTH = 360;
    public const int NOTE_WDW_HEIGHT = 380;

    // PREFS MANAGER
    public const string PRF_COL = "Column";
    public const string PRF_TASK = "Task";
    public const string PRF_NAME = "Name";
    public const string PRF_DESCR = "Descript";
    public const string PRF_OBS = "Obs";
    public const string PRF_WORK = "Work";
    public const string PRF_REST = "Rest";
    public const string PRF_POMO_MODE = "PomodoroMode";
    public const string PRF_TITLE = "Title";
    public const string PRF_BODY = "Body";
    public const string PRF_COL_FORMAT = "{0:s}{1:s}_{2:0}";
    public const string PRF_TASK_FORMAT = "{0:s}{1:s}_{2:0}_{3:0}";
    public const string PRF_NOTE_FORMAT = "{0:s}{1:s}_{2:0}";

    public const string PRF_BASE_WORK = "25";
    public const string PRF_BASE_REST = "5";
}
