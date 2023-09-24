using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommandBase
{
    private string commandId;
    private string commandDescription;
    private string commandFormat;

    public string CommandID { get { return commandId; } }
    public string CommandDescription { get { return commandDescription; } }
    public string CommandFormat { get { return commandFormat; } }

    public DebugCommandBase(string id, string desc, string format)
    {
        commandId = id;
        commandDescription = desc;
        commandFormat = format;
    }
}


public class DebugCommand : DebugCommandBase
{
    private Action command;
    public DebugCommand(string id, string desc, string format, Action command) : base(id, desc, format)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}


public class DebugCommand<T> : DebugCommandBase
{
    private Action<T> command;
    public DebugCommand(string id, string desc, string format, Action<T> command) : base(id, desc, format)
    {
        this.command = command;
    }

    public void Invoke(T value)
    {
        command.Invoke(value);
    }
}
