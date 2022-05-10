using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

public class OzTimerConfig : EditorWindow
{
    private OrganiZer mainWindow;
    private string workTime = OzConsts.EMPTY;
    private string restTime = OzConsts.EMPTY;
    private GUIStyle pomodoroStyle;
    private GUIStyle centeredStyle;
        

    public static void ShowWindow(OrganiZer context)
    {
        OzTimerConfig window = ScriptableObject.CreateInstance(typeof(OzTimerConfig)) as OzTimerConfig;
        window.minSize = new Vector2(OzConsts.POMO_WDW_WIDTH, OzConsts.POMO_WDW_HEIGHT);
        window.maxSize = new Vector2(OzConsts.POMO_WDW_WIDTH, OzConsts.POMO_WDW_HEIGHT);
        window.mainWindow = context;
        window.workTime = OzConsts.EMPTY+OzPrefsManager.LoadWorkTime();
        window.restTime = OzConsts.EMPTY+OzPrefsManager.LoadRestTime();
        window.pomodoroStyle = new GUIStyle(EditorStyles.textField);
        window.pomodoroStyle.fontSize = 22;
        window.centeredStyle = new GUIStyle(GUI.skin.label);
        window.centeredStyle.alignment = TextAnchor.UpperCenter;
        window.ShowUtility();
    }

    public void OnGUI()
{
        // TOP - LABELS
        GUILayout.BeginHorizontal();
        GUILayout.Label(String.Format(OzConsts.MODE_LBL_FORMAT, OzStrings.POMO_WORK), EditorStyles.boldLabel);
        GUILayout.Label(String.Format(OzConsts.MODE_LBL_FORMAT, OzStrings.POMO_REST), EditorStyles.boldLabel);
        GUILayout.EndHorizontal();

        // MIDDLE - INPUT FIELDS
        GUILayout.BeginHorizontal();
        
        workTime = DrawTextField(workTime);
        restTime = DrawTextField(restTime);

        GUILayout.EndHorizontal();

        GUILayout.Label(OzStrings.TIME_UNIT, centeredStyle);

        GUILayout.FlexibleSpace();

        // BOTTOM - BUTTONS/OTHER
        if (ValidateNewTimes())
            hasUnsavedChanges = true;
        else
            hasUnsavedChanges = false;

        if (GUILayout.Button(OzStrings.SAVE, GUILayout.Height(30)) 
            && workTime != OzConsts.EMPTY && restTime != OzConsts.EMPTY)
            SaveChanges();
    }

    public override void SaveChanges()
    {
        if (int.Parse(workTime) > 0 && int.Parse(restTime) > 0)
        {
            if (mainWindow.IsWorkTime)
                mainWindow.UpdatePomodoroTime(int.Parse(workTime));
            else
                mainWindow.UpdatePomodoroTime(int.Parse(restTime));

            OzPrefsManager.SavePomodoroTime(workTime, restTime);
            this.Close();
        }
        else
            EditorUtility.DisplayDialog(OzStrings.INVALID_TIME_HEADER, OzStrings.INVALID_TIME_BODY, OzStrings.INVALID_TIME_BTN);
    }

    private string DrawTextField(string mode)
    {
        mode = EditorGUILayout.TextField(mode, pomodoroStyle, GUILayout.Height(30));
        mode = Regex.Replace(mode, OzConsts.CHAR_FILTER, OzConsts.EMPTY);
        if (mode.Length > OzConsts.POMO_CHAR_LIMIT)
        {
            mode = mode.Remove(mode.Length - 1);
            GUI.FocusControl(null);
        }

        return mode;
    }

    private bool ValidateNewTimes()
    {
        if (workTime != OzConsts.EMPTY && restTime != OzConsts.EMPTY 
            && workTime != OzConsts.ZERO && restTime != OzConsts.ZERO 
            && (OzPrefsManager.ValidateNewPomodoroTimes(workTime, restTime)))
            return true;
        else
            return false;
    }
}
