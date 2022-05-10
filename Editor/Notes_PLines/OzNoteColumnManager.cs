using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OzNoteColumnManager : EditorWindow
{
    private OrganiZer mainWindow;
    private OzNoteColumn columnToEdit;
    private OzFormBox box;

    public static void ShowWindow(OzNoteColumn columnToEdit, OrganiZer context)
    {
        OzNoteColumnManager window = ScriptableObject.CreateInstance(typeof(OzNoteColumnManager)) as OzNoteColumnManager;
        window.minSize = new Vector2(OzConsts.NOTE_WDW_WIDTH, OzConsts.NOTE_WDW_HEIGHT);
        window.maxSize = new Vector2(OzConsts.NOTE_WDW_WIDTH, OzConsts.NOTE_WDW_HEIGHT);
        window.mainWindow = context;
        window.columnToEdit = columnToEdit;
        window.box = new OzFormBox(columnToEdit);
        window.saveChangesMessage = OzStrings.SAVE_CHANGES_MSG;
        window.ShowUtility();
    }

    public void OnGUI()
    {
        box.OnGUI();

        if ((box.Title != columnToEdit.Title || box.Body != columnToEdit.Body 
            && box.Title != OzConsts.EMPTY && box.Body != OzConsts.EMPTY))
            hasUnsavedChanges = true;
        else
            hasUnsavedChanges = false;

        GUILayout.BeginHorizontal();

        if (GUILayout.Button(OzStrings.SAVE, GUILayout.Height(30)))
            SaveChanges();

        if (GUILayout.Button(OzStrings.DELETE, GUILayout.Height(30)))
            DeleteConfirmation();

        GUILayout.EndHorizontal();
    }

    public override void SaveChanges()
    {
        bool decision = EditorUtility.DisplayDialog(
            OzStrings.SAVING + columnToEdit.Type, // title
            OzStrings.SAVE_ITEM_BODY, // description
            OzStrings.CONFIRM_BTN, // OK button
            OzStrings.CANCEL_BTN // Cancel button
        );

        if (decision)
        {
            if (box.Title == OzConsts.EMPTY || box.Body == OzConsts.EMPTY)
            {
                RecreatePopup(this, mainWindow);
                EditorUtility.DisplayDialog(OzStrings.SAVE_ERROR_TITLE, OzStrings.SAVE_ERROR_BODY, OzConsts.OK_BTN);
            }
            else
            {
                columnToEdit.UpdateData(box);
                this.Close();
            }
        }
    }

    private void DeleteConfirmation()
    {
        bool decision = EditorUtility.DisplayDialog(
            OzStrings.DELETING + columnToEdit.Type, // title
            OzStrings.DELETE_ITEM_BODY, // description
            OzStrings.CONFIRM_BTN, // OK button
            OzStrings.CANCEL_BTN // Cancel button
        );

        if (decision)
        {
            mainWindow.DeleteNoteFromList(columnToEdit);
            this.Close();
        }
    }

    private void RecreatePopup(OzNoteColumnManager thisPopup, OrganiZer context)
    {
        this.Close();

        OzNoteColumnManager window = ScriptableObject.CreateInstance(typeof(OzNoteColumnManager)) as OzNoteColumnManager;
        window.minSize = new Vector2(OzConsts.NOTE_WDW_WIDTH, OzConsts.NOTE_WDW_HEIGHT);
        window.maxSize = new Vector2(OzConsts.NOTE_WDW_WIDTH, OzConsts.NOTE_WDW_HEIGHT);
        window.mainWindow = context;
        window.columnToEdit = thisPopup.columnToEdit;
        window.box = thisPopup.box;
        window.saveChangesMessage = OzStrings.SAVE_CHANGES_MSG;
        window.ShowUtility();
    }
}