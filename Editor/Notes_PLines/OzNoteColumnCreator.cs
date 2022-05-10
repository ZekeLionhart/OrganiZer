using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OzNoteColumnCreator : EditorWindow
{
    private OrganiZer mainWindow;
    private OzFormBox box;
    private string type;

    public static void ShowWindow(string type, OrganiZer context)
    {
        OzNoteColumnCreator window = ScriptableObject.CreateInstance(typeof(OzNoteColumnCreator)) as OzNoteColumnCreator;
        window.minSize = new Vector2(OzConsts.NOTE_WDW_WIDTH, OzConsts.NOTE_WDW_HEIGHT);
        window.maxSize = new Vector2(OzConsts.NOTE_WDW_WIDTH, OzConsts.NOTE_WDW_HEIGHT);
        window.mainWindow = context;
        window.type = type;
        window.box = new OzFormBox(type);
        window.saveChangesMessage = OzStrings.SAVE_CHANGES_MSG;
        window.ShowUtility();
    }

    public void OnGUI()
    {
        box.OnGUI();

        if (box.Title != OzConsts.EMPTY || box.Body != OzConsts.EMPTY)
            hasUnsavedChanges = true;
        else
            hasUnsavedChanges = false;

        if (GUILayout.Button(OzStrings.SAVE + type, GUILayout.Height(30)))
            SaveChanges();
    }

    public override void SaveChanges()
    {
        if (box.Title == OzConsts.EMPTY || box.Body == OzConsts.EMPTY)
        {
            RecreatePopup(this, mainWindow);
            EditorUtility.DisplayDialog(OzStrings.SAVE_ERROR_TITLE, OzStrings.SAVE_ERROR_BODY, OzConsts.OK_BTN);
        }
        else
        {
            mainWindow.AddNoteToList(new OzNoteColumn(box, mainWindow));
            this.Close();
        }
    }

    private void RecreatePopup(OzNoteColumnCreator thisPopup, OrganiZer context)
    {
        this.Close();

        OzNoteColumnCreator window = ScriptableObject.CreateInstance(typeof(OzNoteColumnCreator)) as OzNoteColumnCreator;
        window.minSize = new Vector2(OzConsts.NOTE_WDW_WIDTH, OzConsts.NOTE_WDW_HEIGHT);
        window.maxSize = new Vector2(OzConsts.NOTE_WDW_WIDTH, OzConsts.NOTE_WDW_HEIGHT);
        window.mainWindow = context;
        window.type = thisPopup.type;
        window.box = thisPopup.box;
        window.saveChangesMessage = OzStrings.SAVE_CHANGES_MSG;
        window.ShowUtility();
    }
}