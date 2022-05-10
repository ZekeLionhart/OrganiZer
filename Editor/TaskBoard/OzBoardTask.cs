using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OzBoardTask : GUIDraggableObject
{
    private OrganiZer mainWindow;
    private OzBoardColumn parentColumn;
    private string taskName;
    private string taskDescript;
    private string taskObs;
    private bool isLocked;
    private GUIStyle cardStyle;
    
    public OzBoardColumn ParentColumn => parentColumn;
    public string Name => taskName;
    public string Descript => taskDescript;
    public string Obs => taskObs;
    public bool IsLocked => isLocked;

    private OzBoardTask (OzBoardColumn columnContext, OrganiZer windowContext)
    {
        mainWindow = windowContext;
        parentColumn = columnContext;
        taskName = OzStrings.NEW + OzStrings.TASK;
        taskDescript = OzConsts.EMPTY;
        taskObs = OzConsts.EMPTY;
        isLocked = true;
    }

    public static void CreateTask(OzBoardColumn columnContext, OrganiZer windowContext)
    {
        OzBoardTask newTask = new OzBoardTask(columnContext, windowContext);
        
        columnContext.AddTaskToColumn(newTask, columnContext.TaskList.Count);
        windowContext.AddTaskToList(newTask);
        OzPrefsManager.PushTasks(newTask, columnContext, windowContext);
        OzPrefsManager.SaveTaskCard(newTask, windowContext);
        newTask.UpdatePosition();
    }

    public static void LoadTask(string taskName, string taskDescript, string taskObs, OzBoardColumn columnContext, OrganiZer windowContext)
    {
        OzBoardTask newTask = new OzBoardTask(columnContext, windowContext);
        newTask.taskName = taskName;
        newTask.taskDescript = taskDescript;
        newTask.taskObs = taskObs;

        columnContext.AddTaskToColumn(newTask, columnContext.TaskList.Count);
        windowContext.AddTaskToList(newTask);
        newTask.UpdatePosition();
    }

    public void OnGUI()
    {
        Rect drawRect = new Rect(position.x, position.y, OzConsts.TASK_WIDTH, OzConsts.TASK_HEIGHT), dragRect;
        InitStyle();

        GUILayout.BeginArea(drawRect, cardStyle);
        
        GUILayout.BeginHorizontal();
        GUILayout.Label(OzConsts.EMPTY, GUI.skin.textField, GUILayout.Height(16), GUILayout.ExpandWidth(true));
        if (GUILayout.Button(OzStrings.EDIT_BTN, GUILayout.Width(35), GUILayout.Height(16)))
            OzBoardTaskManager.ShowWindow(this, mainWindow);
        GUILayout.EndHorizontal();
        
        dragRect = GUILayoutUtility.GetLastRect();
        dragRect = new Rect(dragRect.x + position.x, dragRect.y + position.y, dragRect.width, dragRect.height);
        
        GUILayout.Label(taskName, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        GUILayout.EndArea();

        Drag(dragRect);

        if (dragging) isLocked = false;
    }

    private void InitStyle()
    {
        if (cardStyle == null)
        {
            cardStyle = new GUIStyle(GUI.skin.box);
            cardStyle.normal.background = MakeTex(OzConsts.TASK_WIDTH, OzConsts.TASK_HEIGHT, 
                new Color(0.25f, 0.25f, 0.25f, 1f));
        }
    }

    private Texture2D MakeTex(int width, int height, Color color)
    {
        Color[] pix = new Color[width * height];
        
        for (int i = 0; i < pix.Length; i++)
            pix[i] = color;
        
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    public void SetNewData(string newName, string newDescript, string newObs)
    {
        taskName = newName;
        taskDescript = newDescript;
        taskObs = newObs;
        OzPrefsManager.SaveTaskCard(this, mainWindow);
    }

    public void UpdatePosition()
    {
        position = new Vector2(
          OzConsts.BASE_SPACE * 2 
            + (OzConsts.COLUMN_WIDTH + 2 * OzConsts.BASE_SPACE + 4) 
            * mainWindow.ColumnList.IndexOf(parentColumn), // X position
          33 + (OzConsts.TASK_HEIGHT + OzConsts.BASE_SPACE) 
            * parentColumn.TaskList.IndexOf(this) // Y position
        );
    }

    public void DeleteTask()
    {
        OzPrefsManager.DeleteTaskCard(this, mainWindow);
        OzPrefsManager.PullTasks(this, parentColumn, mainWindow);
        parentColumn.RemoveTaskFromColumn(this);
        mainWindow.DeleteTaskFromList(this);
    }
    
    private OzBoardColumn FindClosestColumn()
    {
        int index = 0;
        float closestPos = 1000;
        float res;
        
        for (int i = 0; i < mainWindow.ColumnList.Count; i++)
        {
            res = (position.x + OzConsts.TASK_WIDTH / 2) - (
                OzConsts.BASE_SPACE 
                + (OzConsts.BASE_SPACE * 2 + 4 + OzConsts.COLUMN_WIDTH) * i 
                + OzConsts.COLUMN_WIDTH / 2
            );

            if (res < 0) res = res * -1;

            if (res < closestPos)
            {
                closestPos = res;
                index = i;
            }
        }
        
        return mainWindow.ColumnList[index];
    }
    
    private OzBoardTask FindClosestTask()
    {
        int index = 0;
        float closestPos = 500;
        float res;
        
        for (int i = 0; i < parentColumn.TaskList.Count; i++)
        {
            res = position.y - (parentColumn.TaskList[i].Position.y);

            if (res < 0) res = res * -1;

            if (res < closestPos)
            {
                closestPos = res;
                index = i;
            }
        }
        
        return parentColumn.TaskList[index];
    }

    private int GetIndexRelativeToTask(OzBoardTask taskToCompare)
    {
        int index = parentColumn.TaskList.IndexOf(taskToCompare);

        if (position.y - 33.0f > taskToCompare.Position.y)
            index++;

        return index;
    }

    public void MoveToNewPosition()
    {
        int index = 0;

        OzPrefsManager.DeleteTaskCard(this, mainWindow);
        OzPrefsManager.PullTasks(this, parentColumn, mainWindow);

        parentColumn.RemoveTaskFromColumn(this);
        parentColumn = FindClosestColumn();

        if (parentColumn.TaskList.Count != 0)
            index = GetIndexRelativeToTask(FindClosestTask());
            
        parentColumn.AddTaskToColumn(this, index);
        
        isLocked = true;

        OzPrefsManager.PushTasks(this, parentColumn, mainWindow);
        OzPrefsManager.SaveTaskCard(this, mainWindow);
    }
}
