using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

public class OzBoardColumnManager : EditorWindow
{
    private OrganiZer mainWindow;
    private OzBoardColumn columnToManage;
    private string columnName;
    private string columnIndex;
    private string oldIndex;
    private bool isColumnNew;

    public static void ShowCreateWindow(OrganiZer context)
    {
        OzBoardColumnManager window = ScriptableObject.CreateInstance(typeof(OzBoardColumnManager)) as OzBoardColumnManager;
        window.minSize = new Vector2(OzConsts.COL_WDW_WIDTH, OzConsts.COL_WDW_HEIGHT -30);
        window.maxSize = new Vector2(OzConsts.COL_WDW_WIDTH, OzConsts.COL_WDW_HEIGHT -30);
        window.mainWindow = context;
        window.columnName = OzStrings.NEW + OzStrings.COLUMN;
        window.columnIndex = OzConsts.EMPTY + context.ColumnList.Count;
        window.isColumnNew = true;
        window.ShowUtility();
    }

    public static void ShowEditWindow(OzBoardColumn columnToEdit, OrganiZer context)
    {
        OzBoardColumnManager window = ScriptableObject.CreateInstance(typeof(OzBoardColumnManager)) as OzBoardColumnManager;
        window.minSize = new Vector2(OzConsts.COL_WDW_WIDTH, OzConsts.COL_WDW_HEIGHT);
        window.maxSize = new Vector2(OzConsts.COL_WDW_WIDTH, OzConsts.COL_WDW_HEIGHT);
        window.mainWindow = context;
        window.columnToManage = columnToEdit;
        window.columnName = columnToEdit.Name;
        window.columnIndex = OzConsts.EMPTY + context.GetTaskColumnIndex(columnToEdit);
        window.oldIndex = window.columnIndex;
        window.isColumnNew = false;
        window.saveChangesMessage = OzStrings.SAVE_CHANGES_MSG;
        window.ShowUtility();
    }
    
    public void OnGUI()
    {
        GUILayout.Label(OzStrings.COLUMN_NAME_LBL);
        columnName = GUILayout.TextField(columnName, GUILayout.ExpandWidth(true));
        GUILayout.Space(10);

        GUILayout.Label(OzStrings.COLUMN_INDEX_LBL);
        columnIndex = GUILayout.TextField(columnIndex, GUILayout.ExpandWidth(true));
        columnIndex = Regex.Replace(columnIndex, OzConsts.CHAR_FILTER, OzConsts.EMPTY);
        GUILayout.Space(10);

        if (isColumnNew)
            if (GUILayout.Button(OzStrings.SAVE, GUILayout.ExpandHeight(true)))
                SaveChanges();
        if (!isColumnNew)
        {
            if (columnIndex != oldIndex || columnToManage.Name != columnName)
                hasUnsavedChanges = true;
            else
                hasUnsavedChanges = false;

            if (GUILayout.Button(OzStrings.SAVE, GUILayout.ExpandHeight(true)))
                SaveChanges();
            if (GUILayout.Button(OzStrings.DELETE, GUILayout.ExpandHeight(true)))
                DeleteColumn();
        }
    }

    public override void SaveChanges()
    {
        if (isColumnNew) 
        {
            columnToManage = new OzBoardColumn(columnName, mainWindow);
            mainWindow.AddTaskColumnToList(columnToManage, int.Parse(columnIndex));
            this.Close();
        }

        if (!isColumnNew)
        {
            bool decision = EditorUtility.DisplayDialog(
                OzStrings.SAVING + OzStrings.COLUMN, // title
                OzStrings.SAVE_ITEM_BODY, // description
                OzStrings.CONFIRM_BTN, // OK button
                OzStrings.CANCEL_BTN // Cancel button
            );

            if (decision)
            {
                columnToManage.SetName(columnName);

                if (oldIndex != columnIndex)
                {
                    mainWindow.DeleteTaskColumnFromList(columnToManage);
                    mainWindow.AddTaskColumnToList(columnToManage, int.Parse(columnIndex));
                }

                this.Close();
            }
        }
    }

    private void DeleteColumn()
    {
        bool decision = EditorUtility.DisplayDialog(
            OzStrings.DELETING + OzStrings.COLUMN, // title
            OzStrings.SAVE_ITEM_BODY, // description
            OzStrings.CONFIRM_BTN, // OK button
            OzStrings.CANCEL_BTN // Cancel button
        );
        
        if (decision)
        {
            mainWindow.DeleteTaskColumnFromList(columnToManage);
            this.Close();
        }
    }
}