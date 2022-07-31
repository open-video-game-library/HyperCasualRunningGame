using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MU5Attribute
{
    public class ShowIfAttribute : PropertyAttribute
    {
        public string fieldName;
        public ShowIfAttribute(string fieldName)
        {
            this.fieldName = fieldName;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfEditor : PropertyDrawer
    {
        ShowIfAttribute _attribute { get { return attribute as ShowIfAttribute; } }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            List<string> splitPath = SplitPath(property.propertyPath);
            string path = GetPath(splitPath, _attribute.fieldName);
            if (property.serializedObject.FindProperty(path) == null) return;

            SerializedProperty targetProperty = property.serializedObject.FindProperty(path);
            if (targetProperty.boolValue)
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            List<string> splitPath = SplitPath(property.propertyPath);
            string path = GetPath(splitPath, _attribute.fieldName);

            if (property.serializedObject.FindProperty(path) == null) return 0;
            else if (property.serializedObject.FindProperty(path).boolValue) return base.GetPropertyHeight(property, label);
            else return 0;
        }

        List<string> SplitPath(string path)
        {
            List<string> pathList = new List<string>();
            string[] split = path.Split('.');
            for (int i = 0; i < split.Length; i++)
            {
                pathList.Add(split[i]);
            }
            return pathList;
        }

        string GetPath(List<string> splitPath, string name)
        {
            string path = string.Empty;
            for (int i = 0; i < splitPath.Count - 1; i++)
            {
                path += splitPath[i] + ".";
            }
            path += name;
            return path;
        }
    }
#endif
}
