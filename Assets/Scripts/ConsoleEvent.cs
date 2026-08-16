using IngameDebugConsole;
using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

public class ConsoleEvent : MonoBehaviour
{
    public string commandName, commandDescription;
    public bool dontDestroyOnLoad = false;
    public UltEvent CommandEvent;

    // Start is called before the first frame update
    void Start()
    {
        if (dontDestroyOnLoad) DontDestroyOnLoad(this);
        DebugLogConsole.AddCommand(commandName, commandDescription, ConsoleCommand);
    }

    private void ConsoleCommand()
    {
        CommandEvent.Invoke();
    }

    private void OnDestroy()
    {
        DebugLogConsole.RemoveCommand(commandName);
    }
}
