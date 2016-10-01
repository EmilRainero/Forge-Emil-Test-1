using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugLog : MonoBehaviour  {
    private static Text Text { get; set; }

    private static DebugLog _instance;

    public static DebugLog Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public static void SetText(Text text)
    {
        Text = text;
    }
    public static void Log(string message)
    {
        Debug.Log("Adding " + message);
        Text.text += message + "\r\n";
    }
}
