using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OzBoardColumn
{
    private OrganiZer mainWindow;
    private string columnName;
    private int columnHeight;
    private GUIStyle newTaskStyle;
    private List<OzBoardTask> taskList;

    public string Name => columnName;
    public List<OzBoardTask> TaskList => taskList;

    public OzBoardColumn (string newName, OrganiZer context)
    {
        mainWindow = context;
        columnName = newName;
        columnHeight = OzConsts.COLUMN_HEIGHT;
        taskList = new List<OzBoardTask>();
        newTaskStyle = new GUIStyle(GUI.skin.button);
        newTaskStyle.fontSize = 40;
        newTaskStyle.alignment = TextAnchor.UpperCenter;
    }

    public void OnGUI()
    {
        // Main Body
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Label(columnName, GUILayout.Width(161));
        if (GUILayout.Button(OzStrings.EDIT_BTN, GUILayout.Width(35)))
            OzBoardColumnManager.ShowEditWindow(this, mainWindow);
        GUILayout.EndHorizontal();
        GUILayout.Label(OzConsts.EMPTY, GUI.skin.textField, 
            GUILayout.Width(OzConsts.COLUMN_WIDTH), GUILayout.Height(columnHeight));
        GUILayout.EndVertical();

        // New Task Button
        if (GUI.Button(new Rect(
            OzConsts.BASE_SPACE * 2 + mainWindow.GetTaskColumnIndex(this) 
                * (OzConsts.COLUMN_WIDTH + OzConsts.BASE_SPACE * 2 + 4), // X position
            columnHeight - OzConsts.TASK_HEIGHT - OzConsts.BASE_SPACE + 23, // Y position
            OzConsts.TASK_WIDTH, // Width
            OzConsts.TASK_HEIGHT // Height
        ), OzConsts.PLUS, newTaskStyle))
        {
            OzBoardTask.CreateTask(this, mainWindow);
        }
    }

    public void SetName(string newName)
    {
        columnName = newName;
        OzPrefsManager.SaveTaskColumn(this, mainWindow);
    }

    private void UpdateColumnHeight()
    {
        columnHeight = OzConsts.COLUMN_HEIGHT + (OzConsts.COLUMN_HEIGHT - OzConsts.BASE_SPACE) * taskList.Count; 
    }

    public void AddTaskToColumn(OzBoardTask newTask, int index)
    {
        taskList.Insert(index, newTask);
        foreach (OzBoardTask task in taskList)
            task.UpdatePosition();
        UpdateColumnHeight();
    }

    public void RemoveTaskFromColumn(OzBoardTask taskToRemove)
    {
        taskList.Remove(taskToRemove);
        foreach (OzBoardTask task in taskList)
            task.UpdatePosition();
        UpdateColumnHeight();
    }
}