using System;
using System.Collections;
using UnityEngine;
using UnityEditor;

public class OzBoardTaskManager : EditorWindow
{
    private OrganiZer mainWindow;
    private OzBoardTask taskToManage;
    private string taskName;
    private string taskDescrip;
    private string taskObs;
    private Vector2 scroll;
    private GUIStyle nameStyle;
    private GUIStyle textAreaStyle;

    public static void ShowWindow(OzBoardTask task, OrganiZer context)
    {
        OzBoardTaskManager window = ScriptableObject.CreateInstance(typeof(OzBoardTaskManager)) as OzBoardTaskManager;
        window.minSize = new Vector2(OzConsts.TASK_WDW_WIDTH, OzConsts.TASK_WDW_HEIGHT);
        window.maxSize = new Vector2(OzConsts.TASK_WDW_WIDTH, OzConsts.TASK_WDW_HEIGHT);
        window.mainWindow = context;
        window.taskToManage = task;
        window.taskName = task.Name;
        window.taskDescrip = task.Descript;
        window.taskObs = task.Obs;
        window.nameStyle = new GUIStyle(GUI.skin.textField);
        window.nameStyle.wordWrap = true;
        window.nameStyle.fontSize = 18;
        window.textAreaStyle = new GUIStyle(EditorStyles.textArea);
        window.textAreaStyle.wordWrap = true;
        window.saveChangesMessage = OzStrings.SAVE_CHANGES_MSG;
        window.ShowUtility();
    }

    public void OnGUI()
    {
        // Name & Parent Column
        GUILayout.BeginHorizontal();
        GUILayout.Label(OzStrings.PARENT_LBL + taskToManage.ParentColumn.Name);
        GUILayout.FlexibleSpace();
        GUILayout.Label(String.Format(OzConsts.CHAR_LIMIT_FORMAT, taskName.Length, OzConsts.NAME_LIMIT));
        GUILayout.EndHorizontal();

        taskName = GUILayout.TextField(taskName, nameStyle, GUILayout.ExpandWidth(true));

        GUILayout.Space(20);

        // Description
        GUILayout.BeginHorizontal();
        GUILayout.Label(OzStrings.DESCR_LBL);
        GUILayout.FlexibleSpace();
        GUILayout.Label(String.Format(OzConsts.CHAR_LIMIT_FORMAT, taskDescrip.Length, OzConsts.DESCR_LIMIT));
        GUILayout.EndHorizontal();

        scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(150));
        taskDescrip = EditorGUILayout.TextArea(taskDescrip, textAreaStyle, GUILayout.ExpandHeight(true));
        if (taskDescrip.Length > OzConsts.DESCR_LIMIT)
        {
            taskDescrip = taskDescrip.Remove(taskDescrip.Length - 1);
            GUI.FocusControl(null);
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(20);

        // Steps
        GUILayout.BeginHorizontal();
        GUILayout.Label(OzStrings.STEPS_LBL);
        GUILayout.FlexibleSpace();
        GUILayout.Label(String.Format(OzConsts.CHAR_LIMIT_FORMAT, 0, OzConsts.STEPS_LIMIT));
        GUILayout.EndHorizontal();



        GUILayout.Space(20);

        // Observations
        GUILayout.BeginHorizontal();
        GUILayout.Label(OzStrings.OBS_LBL);
        GUILayout.FlexibleSpace();
        GUILayout.Label(String.Format(OzConsts.CHAR_LIMIT_FORMAT, taskObs.Length, OzConsts.OBS_LIMIT));
        GUILayout.EndHorizontal();

        scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(150));

        taskObs = EditorGUILayout.TextArea(taskObs, textAreaStyle, GUILayout.ExpandHeight(true));
        if (taskObs.Length > OzConsts.OBS_LIMIT)
        {
            taskObs = taskObs.Remove(taskObs.Length - 1);
            GUI.FocusControl(null);
        }

        EditorGUILayout.EndScrollView();

        // Buttons
        GUILayout.FlexibleSpace();
        
        if (taskName != taskToManage.Name)
            hasUnsavedChanges = true;
        else
            hasUnsavedChanges = false;

        GUILayout.BeginHorizontal();
        if (GUILayout.Button(OzStrings.SAVE, GUILayout.Height(30)))
            SaveChanges();
        if (GUILayout.Button(OzStrings.DELETE, GUILayout.Height(30)))
            DeleteTask();
        GUILayout.EndHorizontal();
    }

    public override void SaveChanges()
    {
        bool decision = EditorUtility.DisplayDialog(
            OzStrings.SAVING + OzStrings.TASK, // title
            OzStrings.SAVE_ITEM_BODY, // description
            OzStrings.CONFIRM_BTN, // OK button
            OzStrings.CANCEL_BTN // Cancel button
        );
        
        if (decision)
        {
            if (taskName == OzConsts.EMPTY)
            {
                RecreatePopup(this, mainWindow);
                EditorUtility.DisplayDialog(OzStrings.SAVE_ERROR_TITLE, OzStrings.SAVE_ERROR_TASK, OzConsts.OK_BTN);
            }
            else
            {
                taskToManage.SetNewData(taskName, taskDescrip, taskObs);
                this.Close();
            }
        }
    }

    private void DeleteTask()
    {
        bool decision = EditorUtility.DisplayDialog(
            OzStrings.DELETING + OzStrings.TASK, // title
            OzStrings.DELETE_ITEM_BODY, // description
            OzStrings.CONFIRM_BTN, // OK button
            OzStrings.CANCEL_BTN // Cancel button
        );
        
        if (decision)
        {
            taskToManage.DeleteTask();
            this.Close();
        }
    }

    private void RecreatePopup(OzBoardTaskManager thisPopup, OrganiZer context)
    {
        this.Close();

        OzBoardTaskManager window = ScriptableObject.CreateInstance(typeof(OzBoardTaskManager)) as OzBoardTaskManager;
        window.minSize = new Vector2(OzConsts.TASK_WDW_WIDTH, OzConsts.TASK_WDW_HEIGHT);
        window.maxSize = new Vector2(OzConsts.TASK_WDW_WIDTH, OzConsts.TASK_WDW_HEIGHT);
        window.mainWindow = context;
        window.taskToManage = thisPopup.taskToManage;
        window.taskName = thisPopup.taskName;
        window.taskDescrip = thisPopup.taskDescrip;
        window.taskObs = thisPopup.taskObs;
        window.nameStyle = new GUIStyle(GUI.skin.textField);
        window.nameStyle.wordWrap = true;
        window.nameStyle.fontSize = 18;
        window.textAreaStyle = new GUIStyle(EditorStyles.textArea);
        window.textAreaStyle.wordWrap = true;
        window.saveChangesMessage = OzStrings.SAVE_CHANGES_MSG;
        window.ShowUtility();
    }
}