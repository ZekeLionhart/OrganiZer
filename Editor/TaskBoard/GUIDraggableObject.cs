using System.Collections;
using UnityEngine;

public class GUIDraggableObject
{
    protected Vector2 position;
    private Vector2 dragStart;
    protected bool dragging;

    public bool Dragging => dragging;
    public Vector2 Position => position;
    
    protected void Drag (Rect draggingRect)
    {
        if (Event.current.type == EventType.MouseUp)
            dragging = false;
        else if (Event.current.type == EventType.MouseDown 
                && draggingRect.Contains(Event.current.mousePosition))
        {
            dragging = true;
            dragStart = Event.current.mousePosition - position;
            Event.current.Use();
        }

        if (dragging)
        {
            position = Event.current.mousePosition - dragStart;
        }
    }
}
