#if UNITY_EDITOR
using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class OzPomodoroAlert : MonoBehaviour
{
    private string header;
    private string format;
    private string mode;
    private string okBtn;
    private Stopwatch stopWatch;
    private TimeSpan t0;
    private EditorWindow context;

    public Stopwatch StopWatch => stopWatch;

    public void StartPomoTimer(Stopwatch stopWatch, TimeSpan t0, string header, string format, string mode, string okBtn, EditorWindow context)
    {
        this.stopWatch = stopWatch;
        this.t0 = t0;
        this.context = context;
        this.header = header;
        this.format = format;
        this.mode = mode;
        this.okBtn = okBtn;
    }

    private void Update()
    {
        if (stopWatch != null && stopWatch.Elapsed > t0)
        {
            EditorUtility.DisplayDialog(
                header,
                String.Format(format, mode), 
                okBtn
            );
            if (context != null) context.Show();
            stopWatch.Reset();
            stopWatch = null;
            DestroyThis();
        }
        if (context == null && stopWatch != null) 
        {
            stopWatch.Stop();
            DestroyThis();
        }
    }

    public void DestroyThis()
    {
        DestroyImmediate(this.gameObject);
    }
}
#endif