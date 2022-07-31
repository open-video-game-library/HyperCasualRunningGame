using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MU5Attribute;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DesignHelper : MonoBehaviour
{
    [HideInInspector] public int photoNumber = 1;

    public bool useManualName = false;
    [ShowIf(nameof(useManualName))] public string photoName;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

    [ContextMenu("Screen Shot")]
    public void ScreepShot(string _photoName)
    {
        ScreenCapture.CaptureScreenshot($"{_photoName}");
        photoNumber++;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DesignHelper))]
public class DesignHelperDrawer : Editor
{
    DesignHelper designHelper { get { return target as DesignHelper; } }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        string _photoName = string.Empty;
        if (designHelper.useManualName)
        {
            _photoName = designHelper.photoName;
        }
        else
        {
            _photoName = $"image_{designHelper.photoNumber}.png";
            EditorGUILayout.LabelField($"Photo Name : {_photoName}");
        }
        if (GUILayout.Button("Screen Shot"))
        {
            designHelper.ScreepShot(_photoName);
        }
    }
}
#endif