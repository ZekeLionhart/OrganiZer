using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OzNoteColumn
{
    private OrganiZer mainWindow;
    private string titleText;
    private string bodyText;
    private string type;
    private GUIStyle boxStyle;

    public string Title => titleText;
    public string Body => bodyText;
    public string Type => type;

    public OzNoteColumn(OzFormBox box, OrganiZer context)
    {
        mainWindow = context;
        this.type = box.Type;
        titleText = box.Title;
        bodyText = box.Body;
        boxStyle = new GUIStyle(EditorStyles.textArea);
        boxStyle.wordWrap = true;
    }

    public OzNoteColumn(string titleText, string bodyText, string type, OrganiZer context)
    {
        mainWindow = context;
        this.type = type;
        this.titleText = titleText;
        this.bodyText = bodyText;
        boxStyle = new GUIStyle(EditorStyles.textArea);
        boxStyle.wordWrap = true;
    }

    public void OnGUI()
    {
        GUILayout.BeginVertical();
        if (GUILayout.Button(titleText, boxStyle, GUILayout.Width(200)) ||
            GUILayout.Button(bodyText, boxStyle, GUILayout.ExpandHeight(true), GUILayout.Width(200)))
            OzNoteColumnManager.ShowWindow(this, mainWindow);
        GUILayout.Space(10);
        GUILayout.EndVertical();
    }

    public void UpdateData(OzFormBox box)
    {
        titleText = box.Title;
        bodyText = box.Body;
        OzPrefsManager.SaveNoteColumn(this, mainWindow);
    }
}