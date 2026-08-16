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

    void Start()
    {
        if (dontDestroyOnLoad) DontDestroyOnLoad(this);
    }

    private void ConsoleCommand()
    {
        CommandEvent.Invoke();
    }

    private void OnEnable()
    {
        DebugLogConsole.AddCommand(commandName, commandDescription, ConsoleCommand);
    }

    private void OnDisable()
    {
        DebugLogConsole.RemoveCommand(commandName);
    }

    private void OnDestroy()
    {
        DebugLogConsole.RemoveCommand(commandName);
    }
}
