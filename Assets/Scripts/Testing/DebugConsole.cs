/*
 * OnEnable, OnDisable, and HandleLogic code from derHugo on StackOverflow: https://stackoverflow.com/questions/60228993/putting-debug-log-as-a-gui-element-in-unity
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using TMPro;

public class DebugConsole : MonoBehaviour
{
    // Adjust via the Inspector
    public int maxLines = 28;
    public TextMeshProUGUI console;

    private Queue<string> queue = new Queue<string>();

    private void Start()
    {
        console.text = "";
    }

    void OnEnable()
    {
        Application.logMessageReceivedThreaded += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceivedThreaded -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Delete oldest message
        if (queue.Count >= maxLines) queue.Dequeue();

        queue.Enqueue(logString);

        var builder = new StringBuilder();
        foreach (string st in queue)
        {
            builder.Append(st).Append("\n");
        }

        console.text = builder.ToString();
    }
}
