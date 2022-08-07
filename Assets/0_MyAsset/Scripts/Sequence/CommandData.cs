using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CommandData : MonoBehaviour
{
    public UnityEvent command;

    public void Play()
    {
        command.Invoke();
    }
}
