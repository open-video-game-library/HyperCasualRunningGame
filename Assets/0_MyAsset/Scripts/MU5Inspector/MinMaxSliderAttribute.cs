using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MU5Attribute
{
    public class MinMaxSliderAttribute : PropertyAttribute
    {
        public float minLimit;
        public float maxLimit;
        public float minValue;
        public float maxValue;
        public MinMaxSliderAttribute(float minLimit, float maxLimit)
        {
            this.minLimit = minLimit;
            this.maxLimit = maxLimit;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxSliderEditor : PropertyDrawer
    {
        MinMaxSliderAttribute _attribute { get { return attribute as MinMaxSliderAttribute; } }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Vector2) return;

            Rect labelPos = position;
            labelPos.y -= EditorGUIUtility.singleLineHeight / 2;
            float minValue = property.vector2Value.x;
            float maxValue = property.vector2Value.y;
            EditorGUI.LabelField(labelPos, label.text);

            Rect minPos = position;
            minPos.y += EditorGUIUtility.singleLineHeight / 2;
            minPos.width = position.width;
            EditorGUI.LabelField(minPos, $"{minValue:F2}");

            Rect sliderPos = position;
            sliderPos.x += position.width * 0.2f;
            sliderPos.y += EditorGUIUtility.singleLineHeight;
            sliderPos.width = position.width * 0.6f;
            EditorGUI.MinMaxSlider(sliderPos, ref minValue, ref maxValue, _attribute.minLimit, _attribute.maxLimit);
            //EditorGUI.TextField(sliderPos, $"{position.width}");
            property.vector2Value = new Vector2(minValue, maxValue);

            Rect maxPos = position;
            maxPos.x += position.width * 0.8f;
            maxPos.y += EditorGUIUtility.singleLineHeight / 2;
            maxPos.width = position.width * 0.2f;
            EditorGUI.LabelField(maxPos, $"{maxValue:F2}");
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Vector2) return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight;
            else return 0;
        }
    }
#endif
}