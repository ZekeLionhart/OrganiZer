using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class OzPrefsManager
{
    private static List<string> GetTaskAttributes()
    {
        List<string> taskAttributes = new List<string>
        {
            OzConsts.PRF_NAME,
            OzConsts.PRF_DESCR,
            OzConsts.PRF_OBS
        };
        return taskAttributes;
    }

    public static void SaveTaskColumn(OzBoardColumn columnToSave, OrganiZer context)
    {
        EditorPrefs.SetString(String.Format(OzConsts.PRF_COL_FORMAT, OzConsts.PRF_COL, OzConsts.PRF_NAME, 
            context.ColumnList.IndexOf(columnToSave)), columnToSave.Name);
    }

    public static void DeleteTaskColumn(OzBoardColumn columnToDelete, OrganiZer context)
    {
        EditorPrefs.DeleteKey(String.Format(OzConsts.PRF_COL_FORMAT, OzConsts.PRF_COL, OzConsts.PRF_NAME, 
            context.ColumnList.IndexOf(columnToDelete)));
    }

    public static void PushTaskColumns(OzBoardColumn columnInserted, OrganiZer context)
    {
        for (int i = OzConsts.MAX_TASK_COLUMNS; i > context.ColumnList.IndexOf(columnInserted); i--)
        {
            if (EditorPrefs.HasKey(String.Format(
                    OzConsts.PRF_COL_FORMAT, OzConsts.PRF_COL, OzConsts.PRF_NAME, i))
                && !EditorPrefs.HasKey(String.Format(
                    OzConsts.PRF_COL_FORMAT, OzConsts.PRF_COL, OzConsts.PRF_NAME, (i + 1))))
            {
                EditorPrefs.SetString(String.Format(
                    OzConsts.PRF_COL_FORMAT, OzConsts.PRF_COL, OzConsts.PRF_NAME, (i + 1)), 
                    EditorPrefs.GetString(String.Format(
                        OzConsts.PRF_COL_FORMAT,OzConsts.PRF_COL, OzConsts.PRF_NAME, i)));

                EditorPrefs.DeleteKey(String.Format(
                    OzConsts.PRF_COL_FORMAT, OzConsts.PRF_COL, OzConsts.PRF_NAME, i));
            }
        }
    }

    public static void PullTaskColumns(OzBoardColumn columnRemoved, OrganiZer context)
    {
        for (int i = context.ColumnList.IndexOf(columnRemoved); i < OzConsts.MAX_TASK_COLUMNS; i++)
        {
            if (!EditorPrefs.HasKey(String.Format(
                    OzConsts.PRF_COL_FORMAT, OzConsts.PRF_COL, OzConsts.PRF_NAME, i))
                && EditorPrefs.HasKey(String.Format(
                    OzConsts.PRF_COL_FORMAT, OzConsts.PRF_COL, OzConsts.PRF_NAME, (i + 1))))
            {
                EditorPrefs.SetString(String.Format(
                    OzConsts.PRF_COL_FORMAT,OzConsts.PRF_COL, OzConsts.PRF_NAME, i), 
                    EditorPrefs.GetString(String.Format(
                        OzConsts.PRF_COL_FORMAT,OzConsts.PRF_COL, OzConsts.PRF_NAME, (i + 1))));

                EditorPrefs.DeleteKey(String.Format(
                    OzConsts.PRF_COL_FORMAT,OzConsts.PRF_COL, OzConsts.PRF_NAME, (i + 1)));
            }
            else
                break;
        }
    }

    public static List<OzBoardColumn> LoadAllColumns(OrganiZer context)
    {
        List<OzBoardColumn> tempList = new List<OzBoardColumn>();

        for (int i = 0; i < OzConsts.MAX_TASK_COLUMNS; i++)
        {
            if (EditorPrefs.HasKey(String.Format(
                OzConsts.PRF_COL_FORMAT, OzConsts.PRF_COL, OzConsts.PRF_NAME, i)))
            {
                tempList.Add(new OzBoardColumn(EditorPrefs.GetString(String.Format(
                    OzConsts.PRF_COL_FORMAT, OzConsts.PRF_COL, OzConsts.PRF_NAME, i)), context));
            }
            else
                break;
        }

        return tempList;
    }
    
    public static void SaveTaskCard(OzBoardTask taskToSave, OrganiZer context)
    {
        EditorPrefs.SetString(String.Format(OzConsts.PRF_TASK_FORMAT, 
            OzConsts.PRF_TASK, OzConsts.PRF_NAME, context.ColumnList.IndexOf(taskToSave.ParentColumn), 
            taskToSave.ParentColumn.TaskList.IndexOf(taskToSave)), taskToSave.Name);
        
        EditorPrefs.SetString(String.Format(OzConsts.PRF_TASK_FORMAT, 
            OzConsts.PRF_TASK, OzConsts.PRF_DESCR, context.ColumnList.IndexOf(taskToSave.ParentColumn), 
            taskToSave.ParentColumn.TaskList.IndexOf(taskToSave)), taskToSave.Descript);
        
        EditorPrefs.SetString(String.Format(OzConsts.PRF_TASK_FORMAT, 
            OzConsts.PRF_TASK, OzConsts.PRF_OBS, context.ColumnList.IndexOf(taskToSave.ParentColumn), 
            taskToSave.ParentColumn.TaskList.IndexOf(taskToSave)), taskToSave.Obs);
    }

    public static void DeleteTaskCard(OzBoardTask taskToDelete, OrganiZer context)
    {
        EditorPrefs.DeleteKey(String.Format(OzConsts.PRF_TASK_FORMAT, 
            OzConsts.PRF_TASK, OzConsts.PRF_NAME, context.ColumnList.IndexOf(taskToDelete.ParentColumn), 
            taskToDelete.ParentColumn.TaskList.IndexOf(taskToDelete)));

        EditorPrefs.DeleteKey(String.Format(OzConsts.PRF_TASK_FORMAT, 
            OzConsts.PRF_TASK, OzConsts.PRF_DESCR, context.ColumnList.IndexOf(taskToDelete.ParentColumn), 
            taskToDelete.ParentColumn.TaskList.IndexOf(taskToDelete)));

        EditorPrefs.DeleteKey(String.Format(OzConsts.PRF_TASK_FORMAT, 
            OzConsts.PRF_TASK, OzConsts.PRF_OBS, context.ColumnList.IndexOf(taskToDelete.ParentColumn), 
            taskToDelete.ParentColumn.TaskList.IndexOf(taskToDelete)));
    }

    public static void PushTasks(OzBoardTask taskInserted, OzBoardColumn parentColumn, OrganiZer context)
    {
        int columnIndex = context.ColumnList.IndexOf(parentColumn);
        List<string> taskAttributes = GetTaskAttributes();
        
        foreach (string attribute in taskAttributes)
        {
            for (int i = OzConsts.MAX_TASKS; i >= parentColumn.TaskList.IndexOf(taskInserted); i--)
            {
                if (EditorPrefs.HasKey(String.Format(
                        OzConsts.PRF_TASK_FORMAT, OzConsts.PRF_TASK, attribute, columnIndex, i))
                    && !EditorPrefs.HasKey(String.Format(
                        OzConsts.PRF_TASK_FORMAT, OzConsts.PRF_TASK, attribute, columnIndex, (i + 1))))
                {
                    EditorPrefs.SetString(String.Format(
                        OzConsts.PRF_TASK_FORMAT, OzConsts.PRF_TASK, attribute, columnIndex, (i + 1)), 
                        EditorPrefs.GetString(String.Format(
                            OzConsts.PRF_TASK_FORMAT, OzConsts.PRF_TASK, attribute, columnIndex, i)));

                    EditorPrefs.DeleteKey(String.Format(
                        OzConsts.PRF_TASK_FORMAT, OzConsts.PRF_TASK, attribute, columnIndex, i));
                }
            }
        }
    }

    public static void PullTasks(OzBoardTask taskRemoved, OzBoardColumn parentColumn, OrganiZer context)
    {
        int columnIndex = context.ColumnList.IndexOf(parentColumn);
        List<string> taskAttributes = GetTaskAttributes();
        
        foreach (string attribute in taskAttributes)
        {
            for (int i = parentColumn.TaskList.IndexOf(taskRemoved); i < OzConsts.MAX_TASKS; i++)
            {
                if (!EditorPrefs.HasKey(String.Format(
                        OzConsts.PRF_TASK_FORMAT, OzConsts.PRF_TASK, attribute, columnIndex, i))
                    && EditorPrefs.HasKey(String.Format(
                        OzConsts.PRF_TASK_FORMAT, OzConsts.PRF_TASK, attribute, columnIndex, (i + 1))))
                {
                    EditorPrefs.SetString(String.Format(
                        OzConsts.PRF_TASK_FORMAT, OzConsts.PRF_TASK, attribute, columnIndex, i), 
                        EditorPrefs.GetString(String.Format(
                            OzConsts.PRF_TASK_FORMAT, OzConsts.PRF_TASK, attribute, columnIndex, (i + 1))));
                    
                    EditorPrefs.DeleteKey(String.Format(
                        OzConsts.PRF_TASK_FORMAT, OzConsts.PRF_TASK, attribute, columnIndex, (i + 1)));
                }
                else
                    break;
            }
        }
    }
    
    public static void LoadAllTasks(OrganiZer context)
    {
        for (int i = 0; i < context.ColumnList.Count; i++)
        {
            for (int j = 0; j < OzConsts.MAX_TASKS; j++)
            {
                if (EditorPrefs.HasKey(String.Format(OzConsts.PRF_TASK_FORMAT, 
                    OzConsts.PRF_TASK, OzConsts.PRF_NAME, i, j)))
                {
                    OzBoardTask.LoadTask(
                        EditorPrefs.GetString(String.Format(
                            OzConsts.PRF_TASK_FORMAT, OzConsts.PRF_TASK, OzConsts.PRF_NAME, i, j)),
                        EditorPrefs.GetString(String.Format(
                            OzConsts.PRF_TASK_FORMAT, OzConsts.PRF_TASK, OzConsts.PRF_DESCR, i, j)),
                        EditorPrefs.GetString(String.Format(
                            OzConsts.PRF_TASK_FORMAT, OzConsts.PRF_TASK, OzConsts.PRF_OBS, i, j)),
                        context.ColumnList[i],
                        context
                    );
                }
                else
                    break;
            }
        }
    }

    public static void SetUpPomodoro()
    {
        if (!EditorPrefs.HasKey(OzConsts.PRF_WORK))
            EditorPrefs.SetString(OzConsts.PRF_WORK, OzConsts.PRF_BASE_WORK);
        if (!EditorPrefs.HasKey(OzConsts.PRF_REST))
            EditorPrefs.SetString(OzConsts.PRF_REST, OzConsts.PRF_BASE_REST);
        if (!EditorPrefs.HasKey(OzConsts.PRF_POMO_MODE))
            SavePomodoroMode(true);
    }

    public static bool ValidateNewPomodoroTimes(string newWorkTime, string newRestTime)
    {
        if (newWorkTime != EditorPrefs.GetString(OzConsts.PRF_WORK) 
            || newRestTime != EditorPrefs.GetString(OzConsts.PRF_REST))
            return true;
        else
            return false;
    }

    public static void SavePomodoroTime(string workTime, string restTime)
    {
        EditorPrefs.SetString(OzConsts.PRF_WORK, workTime);
        EditorPrefs.SetString(OzConsts.PRF_REST, restTime);
    }

    public static int LoadWorkTime()
    {
        return int.Parse(EditorPrefs.GetString(OzConsts.PRF_WORK));
    }

    public static int LoadRestTime()
    {
        return int.Parse(EditorPrefs.GetString(OzConsts.PRF_REST));
    }

    public static void SavePomodoroMode(bool newMode)
    {
        EditorPrefs.SetBool(OzConsts.PRF_POMO_MODE, newMode);
    }

    public static bool LoadPomodoroMode()
    {
        return EditorPrefs.GetBool(OzConsts.PRF_POMO_MODE);
    }

    public static void SaveNoteColumn(OzNoteColumn columnToSave, OrganiZer context)
    {
        string type = columnToSave.Type;
        int index = context.NoteList.IndexOf(columnToSave);

        EditorPrefs.SetString(String.Format(
            OzConsts.PRF_NOTE_FORMAT, type, OzConsts.PRF_TITLE, index), columnToSave.Title);

        EditorPrefs.SetString(String.Format(
            OzConsts.PRF_NOTE_FORMAT, type, OzConsts.PRF_BODY, index), columnToSave.Body);
    }

    public static void DeleteNoteColumn(OzNoteColumn columnToDelete, OrganiZer context)
    {
        string type = columnToDelete.Type;
        int index = context.NoteList.IndexOf(columnToDelete);
        
        EditorPrefs.DeleteKey(String.Format(OzConsts.PRF_NOTE_FORMAT, type, OzConsts.PRF_TITLE, index));
        EditorPrefs.DeleteKey(String.Format(OzConsts.PRF_NOTE_FORMAT, type, OzConsts.PRF_BODY, index));

        for (int i = index; i < OzConsts.MAX_NOTE_COLUMNS; i++)
        {
            if (!EditorPrefs.HasKey(String.Format(OzConsts.PRF_NOTE_FORMAT, type, OzConsts.PRF_BODY, i)) && EditorPrefs.HasKey(String.Format(OzConsts.PRF_NOTE_FORMAT, type, OzConsts.PRF_BODY, (i + 1))))
            {
                EditorPrefs.SetString(String.Format(
                    OzConsts.PRF_NOTE_FORMAT, type, OzConsts.PRF_TITLE, i), 
                    EditorPrefs.GetString(String.Format(
                        OzConsts.PRF_NOTE_FORMAT, type, OzConsts.PRF_TITLE, (i + 1))));

                EditorPrefs.SetString(String.Format(
                    OzConsts.PRF_NOTE_FORMAT, type, OzConsts.PRF_BODY, i), 
                    EditorPrefs.GetString(String.Format(
                        OzConsts.PRF_NOTE_FORMAT, type, OzConsts.PRF_BODY, (i + 1))));

                EditorPrefs.DeleteKey(String.Format(
                    OzConsts.PRF_NOTE_FORMAT, type, OzConsts.PRF_TITLE, (i + 1)));

                EditorPrefs.DeleteKey(String.Format(
                    OzConsts.PRF_NOTE_FORMAT, type, OzConsts.PRF_BODY, (i + 1)));
            }
        }
    }

    public static List<OzNoteColumn> LoadAllNotes(string type, OrganiZer context)
    {
        List<OzNoteColumn> tempList = new List<OzNoteColumn>();

        for (int i = 0; i < OzConsts.MAX_NOTE_COLUMNS; i++)
        {
            if (EditorPrefs.HasKey(String.Format(OzConsts.PRF_NOTE_FORMAT, type, OzConsts.PRF_BODY, i)))
            {
                tempList.Add(new OzNoteColumn(EditorPrefs.GetString(String.Format(
                    OzConsts.PRF_NOTE_FORMAT, type, OzConsts.PRF_TITLE, i)),
                    EditorPrefs.GetString(String.Format(
                        OzConsts.PRF_NOTE_FORMAT, type, OzConsts.PRF_BODY, i)), type, context));
            }
        }

        return tempList;
    }
}
