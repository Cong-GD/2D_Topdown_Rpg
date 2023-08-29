using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DialogueObject
{

    [TextArea]
    public string[] sentences;
    public List<Option> options;
}

[Serializable]
public class Option
{
    public string message;
    public UnityEvent choosenEvent;
}