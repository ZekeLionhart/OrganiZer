using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OzFormBox
{
    private string titleText = OzConsts.EMPTY;
    private string bodyText = OzConsts.EMPTY;
    private string type;
    private GUIStyle boxStyle;
    private Vector2 scroll;

    public string Type => type;
    public string Title => titleText;
    public string Body => bodyText;

    public OzFormBox(string type)
    {
        this.type = type;
    }

    public OzFormBox(OzNoteColumn columnToManage)
    {
        titleText = columnToManage.Title;
        bodyText = columnToManage.Body;
        type = columnToManage.Type;
    }

    public void OnGUI()
    {
        boxStyle = new GUIStyle(EditorStyles.textArea);
        boxStyle.wordWrap = true;

        scroll = EditorGUILayout.BeginScrollView(scroll);

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Label(OzStrings.TITLE_LBL, EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.Label(String.Format(OzConsts.CHAR_LIMIT_FORMAT, titleText.Length, OzConsts.TITLE_LIMIT));
        GUILayout.EndHorizontal();

        titleText = EditorGUILayout.TextField(titleText, boxStyle);
        if (titleText.Length > OzConsts.TITLE_LIMIT)
        {
            titleText = titleText.Remove(titleText.Length - 1);
            GUI.FocusControl(null);
        }
        
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Label(OzStrings.BODY_LBL, EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.Label(String.Format(OzConsts.CHAR_LIMIT_FORMAT, bodyText.Length, OzConsts.BODY_LIMIT));
        GUILayout.EndHorizontal();

        bodyText = EditorGUILayout.TextArea(bodyText, boxStyle, GUILayout.ExpandHeight(true));
        if (bodyText.Length > OzConsts.BODY_LIMIT)
        {
            bodyText = bodyText.Remove(bodyText.Length - 1);
            GUI.FocusControl(null);
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();
    }
}
