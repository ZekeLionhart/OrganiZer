using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OzStrings
{
    // GENERAL
    public const string CONFIRM_BTN = "Yes";
    public const string CANCEL_BTN = "No";
    public const string SAVE_CHANGES_MSG = "This window has unsaved changes. Would you like to save?";
    public const string SAVE_ERROR_TITLE = "Save error";
    public const string SAVE_ERROR_BODY = "Fill in all fields before saving!";
    public const string SAVE_ERROR_TASK = "Fill in the task's name before saving!";
    public const string SAVE = "Save";
    public const string DELETE = "Delete";
    public const string SAVING = "Saving ";
    public const string DELETING = "Deleting ";
    public const string NEW = "New ";

    // TASK BOARD
    public const string TASK_BOARD = "Task Board";
    public const string TASK = "Task";
    public const string COLUMN = "Column";
    public const string COLUMN_NAME_LBL = "Column's Name:";
    public const string COLUMN_INDEX_LBL = "Column's Index:";
    public const string EDIT_BTN = "Edit";
    public const string PARENT_LBL = "Belongs to: ";
    public const string DESCR_LBL = "Description:";
    public const string STEPS_LBL = "Steps:";
    public const string OBS_LBL = "Observations:";
    public const string SAVE_ITEM_BODY = "Are you sure want to modify this item?";
    public const string DELETE_ITEM_BODY = "Are you sure want to delete this item?\nThis action cannot be undone.";

    // POMODORO
    public const string POMODORO = "Pomodoro";
    public const string POMO_WORK = "Work";
    public const string POMO_CONFIG = "Config.";
    public const string POMO_REST = "Rest";
    public const string TIME_UNIT = "(in minutes)";
    public const string PAUSE_BTN = "Pause";
    public const string START_BTN = "Start";
    public const string RESET_BTN = "Reset";
    public const string SWITCH_MODES_HEADER = "Switching Modes";
    public const string SWITCH_MODES_BODY = "Are you sure want to switch modes?\nProceeding will reset the timer!";
    public const string INVALID_TIME_HEADER = "Invalid time";
    public const string INVALID_TIME_BODY = "You cannot work or rest for zero minutes!";
    public const string INVALID_TIME_BTN = "OK, I'll enter a valid time";
    public const string POMO_TIMER_HEADER = "Pomodoro Timer";
    public const string POMO_TIMER_FORMAT = "The timer has run out! Your time to {0:s} is over!";

    // NOTES-PIPELINES
    public const string PIPELINES = "Pipelines";
    public const string PIPELINE = "Pipeline";
    public const string NOTES = "Notes";
    public const string NOTE = "Note";
    public const string TITLE_LBL = "Title:";
    public const string BODY_LBL = "Body:";
}
