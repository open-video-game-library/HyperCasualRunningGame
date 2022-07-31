using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MU5Attribute
{
    public class FieldBoxValueAttribute : PropertyAttribute
    {
        public string fieldName;
        public FieldBoxValueAttribute(string fieldName)
        {
            this.fieldName = fieldName;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(FieldBoxValueAttribute))]
    public class FieldBoxValueDrawer : PropertyDrawer
    {
        FieldBoxValueAttribute _attribute { get { return attribute as FieldBoxValueAttribute; } }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            List<string> splitPath = SplitPath(property.propertyPath);
            string path = GetPath(splitPath, _attribute.fieldName);
            path += ".fieldType";
            Debug.Log(path);
            if (property.serializedObject.FindProperty(path) == null) return;

            var targetProperty = property.serializedObject.FindProperty(path);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            List<string> splitPath = SplitPath(property.propertyPath);
            string path = GetPath(splitPath, _attribute.fieldName);
            if (property.serializedObject.FindProperty(path) == null) return 0;
            else return base.GetPropertyHeight(property, label);
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