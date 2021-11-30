using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Logger : MonoBehaviour
{
    private static int s_VisibleMessageCount;

    private static Text s_LogText;

    private static List<string> s_Log = new List<string>();

    private static readonly StringBuilder s_StringBuilder = new StringBuilder();

    [SerializeField] private Text m_LogText;

    [SerializeField] private int m_VisibleMessageCount = 40;

    private int m_LastMessageCount;

    public Text logText
    {
        get => s_LogText;
        set
        {
            m_LogText = value;
            s_LogText = value;
        }
    }

    public int visibleMessageCount
    {
        get => s_VisibleMessageCount;
        set
        {
            m_VisibleMessageCount = value;
            s_VisibleMessageCount = value;
        }
    }

    private void Awake()
    {
        s_LogText = m_LogText;
        s_VisibleMessageCount = m_VisibleMessageCount;
        Log("Log console initialized.");
    }

    private void Update()
    {
        lock (s_Log)
        {
            if (m_LastMessageCount != s_Log.Count)
            {
                s_StringBuilder.Clear();
                var startIndex = Mathf.Max(s_Log.Count - s_VisibleMessageCount, 0);
                for (var i = startIndex; i < s_Log.Count; ++i) s_StringBuilder.Append($"{i:000}> {s_Log[i]}\n");

                s_LogText.text = s_StringBuilder.ToString();
            }

            m_LastMessageCount = s_Log.Count;
        }
    }

    public static void Log(string message)
    {
        lock (s_Log)
        {
            if (s_Log == null)
                s_Log = new List<string>();

            s_Log.Add(message);
        }
    }
}