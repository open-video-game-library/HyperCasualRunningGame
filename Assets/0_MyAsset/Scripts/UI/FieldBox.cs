using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;
using MU5Attribute;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public enum FieldType
{
    Label,
    Int,
    Float,
    Bool,
    String,
    Popup,
    Color,
    Vector3,
    Texture,
    Button
}

[System.Serializable]
public class Condition
{
    public FieldBox _fieldBox;
    public enum Operator
    {
        [InspectorName("==")] Equals,
        [InspectorName("!=")] NotEqual,
        [InspectorName(">")] GraterThan,
        [InspectorName(">=")] GraterThanOrEqual,
        [InspectorName("<")] LessThan,
        [InspectorName("<=")] LessThanOrEqual
    }
    public Operator _operator = Operator.Equals;
    public object value;
    [HideInInspector] public int intValue;
    [HideInInspector] public float floatValue;
    [HideInInspector] public bool boolValue;
    [HideInInspector] public string stringValue;
    [HideInInspector] public Color colorValue;
    [HideInInspector] public Vector3 vector3Value;
    public bool _condition
    {
        get
        {
            if (_fieldBox == null) return true;
            if (_fieldBox.fieldType == FieldType.Int)
            {
                if (_operator == Operator.Equals) return (int)_fieldBox.value == intValue;
                else if (_operator == Operator.NotEqual) return (int)_fieldBox.value != intValue;
                else if (_operator == Operator.GraterThan) return (int)_fieldBox.value > intValue;
                else if (_operator == Operator.GraterThanOrEqual) return (int)_fieldBox.value >= intValue;
                else if (_operator == Operator.LessThan) return (int)_fieldBox.value < intValue;
                else if (_operator == Operator.LessThanOrEqual) return (int)_fieldBox.value <= intValue;
                else return true;
            }
            else if (_fieldBox.fieldType == FieldType.Float)
            {
                if (_operator == Operator.Equals) return (float)_fieldBox.value == floatValue;
                else if (_operator == Operator.NotEqual) return (float)_fieldBox.value != floatValue;
                else if (_operator == Operator.GraterThan) return (float)_fieldBox.value > floatValue;
                else if (_operator == Operator.GraterThanOrEqual) return (float)_fieldBox.value >= floatValue;
                else if (_operator == Operator.LessThan) return (float)_fieldBox.value < floatValue;
                else if (_operator == Operator.LessThanOrEqual) return (float)_fieldBox.value <= floatValue;
                else return true;
            }
            else if (_fieldBox.fieldType == FieldType.Bool)
            {
                if (_operator == Operator.Equals) return (bool)_fieldBox.value == boolValue;
                else if (_operator == Operator.NotEqual) return (bool)_fieldBox.value != boolValue;
                else return true;
            }
            else if (_fieldBox.fieldType == FieldType.String)
            {
                if (_operator == Operator.Equals) return (string)_fieldBox.value == stringValue;
                else if (_operator == Operator.NotEqual) return (string)_fieldBox.value != stringValue;
                else return true;
            }
            else if (_fieldBox.fieldType == FieldType.Color)
            {
                if (_operator == Operator.Equals) return (Color)_fieldBox.value == colorValue;
                else if (_operator == Operator.NotEqual) return (Color)_fieldBox.value != colorValue;
                else return true;
            }
            else if (_fieldBox.fieldType == FieldType.Vector3)
            {
                if (_operator == Operator.Equals) return (Vector3)_fieldBox.value == vector3Value;
                else if (_operator == Operator.NotEqual) return (Vector3)_fieldBox.value != vector3Value;
                else return true;
            }
            else return true;
        }
    }
}

public class FieldBox : MonoBehaviour
{
    [SerializeField] TMP_Text fieldName_tmpTxt;
    [SerializeField] List<TMP_InputField> inputFields = new List<TMP_InputField>();
    [SerializeField] List<Slider> sliders = new List<Slider>();
    [SerializeField] List<Toggle> toggles = new List<Toggle>();
    [SerializeField] List<Image> images = new List<Image>();
    [SerializeField] List<Button> buttons = new List<Button>();
    [SerializeField] List<TMP_Dropdown> dropdowns = new List<TMP_Dropdown>();

    [Space(30), Header("FieldBox Info")]
    public FieldType fieldType = FieldType.Int;
    public string fieldName;
    [Space(20)]
    public UnityEvent onFieldBoxValueChanged;
    [Space(20)]
    public Condition showIfCondition;

    [HideInInspector] public object value;
    RectTransform rectTransform;
    float defaultHeight = 0;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
        defaultHeight = rectTransform.sizeDelta.y;
        if (fieldType == FieldType.Button)
        {
            buttons[0].onClick.AddListener(() => { onFieldBoxValueChanged.Invoke(); });
        }
    }
    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(showIfCondition._condition);
            Vector2 targetSize = rectTransform.sizeDelta;
            if (showIfCondition._condition) targetSize.y = defaultHeight;
            else targetSize.y = 0;
            rectTransform.sizeDelta = targetSize;
        }
    }
    void OnValidate()
    {
        fieldName_tmpTxt.text = fieldName;
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void SetValue(object value)
    {
        this.value = value;
        if (fieldType == FieldType.Int || fieldType == FieldType.Float || fieldType == FieldType.String)
        {
            inputFields[0].text = $"{value}";
        }
        else if (fieldType == FieldType.Popup)
        {
            dropdowns[0].value = (int)value;
        }
        else if (fieldType == FieldType.Color)
        {
            Color c = (Color)value;
            sliders[0].value = (int)(c.r * 255);
            inputFields[0].text = $"{(int)(c.r * 255)}";
            sliders[1].value = (int)(c.g * 255);
            inputFields[1].text = $"{(int)(c.g * 255)}";
            sliders[2].value = (int)(c.b * 255);
            inputFields[2].text = $"{(int)(c.b * 255)}";
            sliders[3].value = (int)(c.a * 255);
            inputFields[3].text = $"{(int)(c.a * 255)}";
        }
        else if (fieldType == FieldType.Vector3)
        {
            Vector3 v3 = (Vector3)value;
            inputFields[0].text = $"{v3.x}";
            inputFields[1].text = $"{v3.y}";
            inputFields[2].text = $"{v3.z}";
        }
        else if (fieldType == FieldType.Bool)
        {
            toggles[0].isOn = (bool)value;
        }
    }

    public void OnFieldBoxValueChanged(int id)
    {
        if (fieldType == FieldType.Int && int.TryParse(inputFields[0].text, out int intValue)) value = intValue;
        else if (fieldType == FieldType.Float && float.TryParse(inputFields[0].text, out float floatValue)) value = floatValue;
        else if (fieldType == FieldType.Bool) value = toggles[0].isOn;
        else if (fieldType == FieldType.String) value = inputFields[0].text;
        else if (fieldType == FieldType.Popup) value = dropdowns[0].value;
        else if (fieldType == FieldType.Color) OnColorFieldBoxChanged(id);
        else if (fieldType == FieldType.Vector3) OnVector3FieldBoxChanged();
        onFieldBoxValueChanged.Invoke();
    }

    void OnColorFieldBoxChanged(int id)
    {
        if (id < sliders.Count) inputFields[id].text = $"{sliders[id].value:F0}";
        else if (float.TryParse(inputFields[id - sliders.Count].text, out float num))
        {
            if (num < 0)
            {
                inputFields[id - sliders.Count].text = "0";
                num = 0;
            }
            else if (num > 255)
            {
                inputFields[id - sliders.Count].text = "255";
                num = 255;
            }
            sliders[id - sliders.Count].value = num;
        }
        value = new Color(sliders[0].value / 255f, sliders[1].value / 255f, sliders[2].value / 255f, sliders[3].value / 255f);
        images[0].color = (Color)value;
    }

    void OnVector3FieldBoxChanged()
    {
        float x = 0, y = 0, z = 0;
        if (float.TryParse(inputFields[0].text, out float _x)) x = _x;
        if (float.TryParse(inputFields[1].text, out float _y)) y = _y;
        if (float.TryParse(inputFields[2].text, out float _z)) z = _z;
        value = new Vector3(x, y, z);
    }
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(FieldBox))]
public class FieldBoxEditor : Editor
{
    FieldBox fieldBox { get { return target as FieldBox; } }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        EditorGUI.indentLevel++;
        JudgeShowIfCondition();
        if (EditorApplication.isPlaying) EditorGUILayout.LabelField($"is {fieldBox.showIfCondition._condition}");
        serializedObject.ApplyModifiedProperties();
        if (EditorGUI.EndChangeCheck())
        {
            var scene = SceneManager.GetActiveScene();
            EditorSceneManager.MarkSceneDirty(scene);
        }
    }

    #region JudgeShowIfCondition
    public void JudgeShowIfCondition()
    {
        FieldBox _fieldBox = fieldBox.showIfCondition._fieldBox;
        if (_fieldBox == null)
        {
            EditorGUILayout.HelpBox("No FieldBox is set.", MessageType.Info);
            return;
        }
        Condition.Operator _operator = fieldBox.showIfCondition._operator;
        object _value = fieldBox.showIfCondition.value;

        JudgeShowIfCondition_Label(_fieldBox, _operator, _value);
        JudgeShowIfCondition_Int(_fieldBox, _operator, _value);
        JudgeShowIfCondition_Float(_fieldBox, _operator, _value);
        JudgeShowIfCondition_Bool(_fieldBox, _operator, _value);
        JudgeShowIfCondition_String(_fieldBox, _operator, _value);
        JudgeShowIfCondition_Popup(_fieldBox, _operator, _value);
        JudgeShowIfCondition_Color(_fieldBox, _operator, _value);
        JudgeShowIfCondition_Vector3(_fieldBox, _operator, _value);

    }
    void JudgeShowIfCondition_Label(FieldBox _fieldBox, Condition.Operator _operator, object _value)
    {
        if (fieldBox.fieldType != FieldType.Label) return;
        EditorGUILayout.HelpBox("FieldBox type \"Label\" is not surported.", MessageType.Warning);
    }
    void JudgeShowIfCondition_Int(FieldBox _fieldBox, Condition.Operator _operator, object _value)
    {
        if (_fieldBox.fieldType != FieldType.Int) return;
        fieldBox.showIfCondition.intValue = EditorGUILayout.IntField("Condition", fieldBox.showIfCondition.intValue);
    }
    void JudgeShowIfCondition_Float(FieldBox _fieldBox, Condition.Operator _operator, object _value)
    {
        if (_fieldBox.fieldType != FieldType.Float) return;
        fieldBox.showIfCondition.floatValue = EditorGUILayout.FloatField("Condition", fieldBox.showIfCondition.floatValue);
    }
    void JudgeShowIfCondition_Bool(FieldBox _fieldBox, Condition.Operator _operator, object _value)
    {
        if (_fieldBox.fieldType != FieldType.Bool) return;
        fieldBox.showIfCondition.boolValue = EditorGUILayout.Toggle("Condition", fieldBox.showIfCondition.boolValue);
    }
    void JudgeShowIfCondition_String(FieldBox _fieldBox, Condition.Operator _operator, object _value)
    {
        if (_fieldBox.fieldType != FieldType.String) return;
        fieldBox.showIfCondition.stringValue = EditorGUILayout.TextField("Condition", fieldBox.showIfCondition.stringValue);
    }
    void JudgeShowIfCondition_Popup(FieldBox _fieldBox, Condition.Operator _operator, object _value)
    {
        if (_fieldBox.fieldType != FieldType.Popup) return;
        EditorGUILayout.HelpBox("Sorry, this FieldBox type isn't surported yet.", MessageType.Warning);
    }
    void JudgeShowIfCondition_Color(FieldBox _fieldBox, Condition.Operator _operator, object _value)
    {
        if (_fieldBox.fieldType != FieldType.Color) return;
        fieldBox.showIfCondition.colorValue = EditorGUILayout.ColorField("Condition", fieldBox.showIfCondition.colorValue);
    }
    void JudgeShowIfCondition_Vector3(FieldBox _fieldBox, Condition.Operator _operator, object _value)
    {
        if (_fieldBox.fieldType != FieldType.Vector3) return;
        fieldBox.showIfCondition.vector3Value = EditorGUILayout.Vector3Field("Condition", fieldBox.showIfCondition.vector3Value);
    }
    void JudgeShowIfCondition_Texture(FieldBox _fieldBox, Condition.Operator _operator, object _value)
    {
        if (_fieldBox.fieldType != FieldType.Texture) return;
        EditorGUILayout.HelpBox("FieldBox type \"Texture\" is not surported.", MessageType.Warning);
    }
    #endregion
}
#endif
#endregion