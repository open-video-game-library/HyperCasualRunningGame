using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SequenceManager : MonoBehaviour
{
    public static SequenceManager i;

    public List<CommandData> commands = new List<CommandData>();

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void Awake()
    {
        i = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Execute();
        }
    }

    public void Execute()
    {
        commands[0].Play();
        commands.RemoveAt(0);
    }
}
//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

#if UNITY_EDITOR
[CustomEditor(typeof(SequenceManager))]
public class SequenceManagerDrawer : Editor
{
    SequenceManager _target { get { return (SequenceManager)target; } }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.HelpBox("If you press Enter/Return,\none Command will be executed.", MessageType.Info);

        if (Application.isPlaying)
        {
            EditorGUILayout.Space(10);
            if (GUILayout.Button("Execute"))
            {
                _target.Execute();
            }
        }
    }
}
#endif
