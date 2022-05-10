using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OrganiZer : EditorWindow
{
    private string[] toolbarOptions = { OzStrings.TASK_BOARD, OzStrings.PIPELINES, OzStrings.POMODORO, OzStrings.NOTES };
    private int toolbarInt = 0;
    private int baseTime = -1;
    private bool isWorkTime = true;
    private Vector2 scroll;
    private GUIStyle pomodoroStyle;
    private Stopwatch stopWatch = new Stopwatch();
    private TimeSpan t0;
    private List<OzBoardColumn> columnList = new List<OzBoardColumn>();
    private List<OzBoardTask> taskList = new List<OzBoardTask>();
    private List<OzNoteColumn> noteList = new List<OzNoteColumn>();

    public List<OzBoardColumn> ColumnList => columnList;
    public List<OzNoteColumn> NoteList => noteList;
    public bool IsWorkTime => isWorkTime;
    public Vector2 Scroll => scroll;

    [MenuItem(OzConsts.MENU_ITEM)]
    public static void Init()
    {
        OrganiZer main = (OrganiZer)EditorWindow.GetWindowWithRect(typeof(OrganiZer), new Rect(
            (Screen.width - OzConsts.MAIN_WIDTH) / 2, 
            (Screen.height - OzConsts.MAIN_HEIGHT) / 2, 
            OzConsts.MAIN_WIDTH, 
            OzConsts.MAIN_HEIGHT
        ));
    }

    public void OnGUI()
    {
        toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarOptions);

        //Toolbar selection
        switch (toolbarInt)
        {
            case 0:
                TaskContent();
                break;
            case 1:
                NotePLineContent(OzStrings.PIPELINE);
                break;
            case 2:
                PomodoroContent();
                break;
            case 3:
                NotePLineContent(OzStrings.NOTE);
                break;
        }
    }

    //Content inside the Task Board tab
    private void TaskContent()
    {
        OzBoardTask toFront = null;

        ControlScrollWithMouse(50);

        if (columnList.Count == 0)
            columnList = OzPrefsManager.LoadAllColumns(this);

        if (columnList.Count > 0 && taskList.Count == 0)
        {
            OzPrefsManager.LoadAllTasks(this);
        }
        
        GUILayout.Space(10);

        scroll = EditorGUILayout.BeginScrollView(scroll);
        GUILayout.BeginHorizontal();
        foreach (OzBoardColumn column in columnList)
        {
            GUILayout.Space(10);
            column.OnGUI();
            GUILayout.Space(10);
        }

        GUILayout.Space(10);
        if (GUILayout.Button(String.Format(OzConsts.NEW_COL_FORMAT, OzStrings.NEW, OzStrings.COLUMN), GUILayout.ExpandHeight(true), GUILayout.Width(80)))
            OzBoardColumnManager.ShowCreateWindow(this);
        GUILayout.Space(10);

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        
        foreach (OzBoardTask task in taskList)
        {
            Color color = GUI.color;

            if (task.Dragging) GUI.color = Color.gray;

            task.OnGUI();

            GUI.color = color;

            if (task.Dragging)
                if (taskList.IndexOf(task) != taskList.Count - 1)
                    toFront = task;

            if (!task.Dragging && !task.IsLocked)
                task.MoveToNewPosition();
        }
        
        if (toFront != null)
        // Move an object to front if needed
        {
            taskList.Remove(toFront);
            taskList.Add(toFront);
        }

        EditorGUILayout.EndScrollView();
    }

    //Content inside the Pipelines and Notes tabs
    private void NotePLineContent(string type)
    {
        PopulateColumnList(type);

        ControlScrollWithMouse(30);
        
        GUILayout.Space(15);
        GUILayout.Label(String.Format(OzConsts.NOTE_LIMIT_FORMAT, type, noteList.Count, OzConsts.MAX_NOTE_COLUMNS),
            EditorStyles.boldLabel);
        GUILayout.Space(15);
        
        scroll = EditorGUILayout.BeginScrollView(scroll);
        GUILayout.BeginHorizontal();

        foreach (OzNoteColumn column in noteList)
        {
            GUILayout.Space(OzConsts.BASE_SPACE);
            column.OnGUI();
            GUILayout.Space(OzConsts.BASE_SPACE);
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button(OzStrings.NEW + type, GUILayout.Height(30)))
            OzNoteColumnCreator.ShowWindow(type, this);
    }

    // Content inside the Pomodoro tab
    private void PomodoroContent()
    {
        pomodoroStyle = new GUIStyle(GUI.skin.label);
        pomodoroStyle.fontSize = 60;
        pomodoroStyle.normal.textColor = Color.white;
        t0 = new TimeSpan(0, baseTime, 0);
        TimeSpan res = t0.Subtract(stopWatch.Elapsed);
        
        SetUpPomodoro();
        
        // What to do when the timer runs out
        if (stopWatch.Elapsed > t0)
        {
            stopWatch.Stop();
            toolbarInt = 2;
            res = new TimeSpan(0, 0, 0);
        }

        // Building area that contains the pomodoro
        GUILayout.BeginArea(new Rect(
            (position.width - OzConsts.MAIN_WIDTH) / 2,
            OzConsts.POMO_WDW_POS_Y,
            OzConsts.MAIN_WIDTH - 3,
            position.height - OzConsts.POMO_WDW_POS_Y - 3
        ));

        // TOP - Modes & config
        GUILayout.BeginHorizontal();

        if (GUILayout.Button(OzStrings.POMO_WORK, GUILayout.Height(30)) && !isWorkTime)
            SwitchPomodoroMode();

        if (GUILayout.Button(OzStrings.POMO_CONFIG, GUILayout.Height(30), GUILayout.Width(50)))
            OzTimerConfig.ShowWindow(this);

        if (GUILayout.Button(OzStrings.POMO_REST, GUILayout.Height(30)) && isWorkTime)
            SwitchPomodoroMode();

        GUILayout.EndHorizontal();


        //MIDDLE - Timer
        string elapsedTime = String.Format(OzConsts.TIMER_FORMAT, res.Hours, res.Minutes, res.Seconds);

        GUILayout.FlexibleSpace();
        
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(elapsedTime, pomodoroStyle);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.FlexibleSpace();


        //BOTTOM - Controls
        GUILayout.BeginHorizontal();

        if (GUILayout.Button(OzStrings.PAUSE_BTN, GUILayout.Height(30)))
            stopWatch.Stop();

        if (GUILayout.Button(OzStrings.START_BTN, GUILayout.Height(30)))
        {
            stopWatch.Start();
            SetTimer();
        }

        if (GUILayout.Button(OzStrings.RESET_BTN, GUILayout.Height(30)))
            stopWatch.Reset();

        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    public void AddTaskColumnToList(OzBoardColumn newColumn, int index)
    {
        if (index < columnList.Count)
        {
            columnList.Insert(index, newColumn);
            OzPrefsManager.PushTaskColumns(newColumn, this);
        }
        else
        {
            index = columnList.Count;
            columnList.Add(newColumn);
        }
        OzPrefsManager.SaveTaskColumn(newColumn, this);
    }

    public void DeleteTaskColumnFromList(OzBoardColumn columnToDelete)
    {
        OzPrefsManager.DeleteTaskColumn(columnToDelete, this);
        OzPrefsManager.PullTaskColumns(columnToDelete, this);
        columnList.Remove(columnToDelete);
    }

    public int GetTaskColumnIndex(OzBoardColumn columnToSearch)
    {
        return columnList.IndexOf(columnToSearch);
    }

    public void AddTaskToList(OzBoardTask newTask)
    {
        taskList.Add(newTask);
    }

    public void DeleteTaskFromList(OzBoardTask taskToDelete)
    {
        taskList.Remove(taskToDelete);
    }

    private void SetTimer()
    {
        string mode;
        
        if (!GameObject.FindObjectOfType<OzPomodoroAlert>())
            PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath(OzConsts.PREFAB_PATH, typeof(GameObject)));
        
        if (isWorkTime) mode = OzStrings.POMO_WORK;
        else mode = OzStrings.POMO_REST;

        GameObject.FindObjectOfType<OzPomodoroAlert>().StartPomoTimer(
            stopWatch, 
            t0, 
            OzStrings.POMO_TIMER_HEADER,
            OzStrings.POMO_TIMER_FORMAT,
            mode,
            OzConsts.OK_BTN,
            this
        );
    }

    private void SetUpPomodoro()
    {
        if (baseTime == -1) 
        {
            OzPrefsManager.SetUpPomodoro();
            isWorkTime = OzPrefsManager.LoadPomodoroMode();
            if (isWorkTime)
                baseTime = OzPrefsManager.LoadWorkTime();
            else
                baseTime = OzPrefsManager.LoadRestTime();
        }
    }

    public void UpdatePomodoroTime(int newTime)
    {
        baseTime = newTime;
    }

    private void SwitchPomodoroMode()
    {
        bool decision = true;

        if (stopWatch.IsRunning)
        {
            decision = EditorUtility.DisplayDialog(
                OzStrings.SWITCH_MODES_HEADER, // title
                OzStrings.SWITCH_MODES_BODY, // description
                OzStrings.CONFIRM_BTN, // OK button
                OzStrings.CANCEL_BTN // Cancel button
            );
        }

        if (decision)
        {
            if (!isWorkTime)
            {
                isWorkTime = true;
                baseTime = OzPrefsManager.LoadWorkTime();
            }
            else
            {
                isWorkTime = false;
                baseTime = OzPrefsManager.LoadRestTime();
            }
            OzPrefsManager.SavePomodoroMode(isWorkTime);
            stopWatch.Reset();
        }
    }

    public void AddNoteToList(OzNoteColumn newColumn)
    {
        noteList.Add(newColumn);
        OzPrefsManager.SaveNoteColumn(newColumn, this);
    }

    public void DeleteNoteFromList(OzNoteColumn columnToDelete)
    {
        OzPrefsManager.DeleteNoteColumn(columnToDelete, this);
        noteList.Remove(columnToDelete);
    }

    private void PopulateColumnList(string type)
    {
        noteList = OzPrefsManager.LoadAllNotes(type, this);
    }

    private void ControlScrollWithMouse(int hitBoxSize)
    {
        Rect y0 = new Rect(0, 0, position.width, hitBoxSize);
        Rect y1 = new Rect(0, position.height - hitBoxSize, position.width, hitBoxSize);
        Rect x0 = new Rect(0, 0, hitBoxSize, position.height);
        Rect x1 = new Rect(position.width - hitBoxSize, 0, hitBoxSize, position.height);
        if (y0.Contains(Event.current.mousePosition)) scroll.y -= 8;
        if (y1.Contains(Event.current.mousePosition)) scroll.y += 8;
        if (x0.Contains(Event.current.mousePosition)) scroll.x -= 8;
        if (x1.Contains(Event.current.mousePosition)) scroll.x += 8;
    }
}