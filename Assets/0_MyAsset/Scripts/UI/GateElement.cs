using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateElement : MonoBehaviour
{
    [SerializeField] GateManager gate;

    [Space(20)]
    [SerializeField] FieldBox position_fieldBox;


    [Space(10)]
    [SerializeField] FieldBox left_calculateMode_fieldBox;
    [SerializeField] FieldBox left_number_fieldBox;

    [Space(10)]
    [SerializeField] FieldBox right_calculateMode_fieldBox;
    [SerializeField] FieldBox right_number_fieldBox;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void Start()
    {
        Initialize();
        gate.gameObject.SetActive(gameObject.activeSelf);
    }

    void OnEnable()
    {
        if (gate != null) gate.gameObject.SetActive(gameObject.activeSelf);
    }

    void OnDisable()
    {
        if (gate != null) gate.gameObject.SetActive(gameObject.activeSelf);
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void Initialize()
    {
        position_fieldBox.SetValue(gate.transform.position);
        right_calculateMode_fieldBox.SetValue((int)gate.gate_R.calculateMode);
        right_number_fieldBox.SetValue(gate.gate_R.number);
        left_calculateMode_fieldBox.SetValue((int)gate.gate_L.calculateMode);
        left_number_fieldBox.SetValue(gate.gate_L.number);
    }

    public void OnFieldBoxValueChanged_position()
    {
        gate.transform.position = (Vector3)position_fieldBox.value;
    }

    public void OnFieldBoxValueChanged_left_calculateMode()
    {
        CalculateMode _calculateMode = (CalculateMode)Enum.ToObject(typeof(CalculateMode), (int)left_calculateMode_fieldBox.value);
        gate.gate_L.SetFormula(_calculateMode, gate.gate_L.number);
    }

    public void OnFieldBoxValueChanged_left_number()
    {
        gate.gate_L.number = (int)left_number_fieldBox.value;
        gate.gate_L.SetFormula(gate.gate_L.calculateMode, gate.gate_L.number);
    }

    public void OnFieldBoxValueChanged_right_calculateMode()
    {
        CalculateMode _calculateMode = (CalculateMode)Enum.ToObject(typeof(CalculateMode), (int)right_calculateMode_fieldBox.value);
        gate.gate_R.SetFormula(_calculateMode, gate.gate_R.number);
    }

    public void OnFieldBoxValueChanged_right_number()
    {
        gate.gate_R.number = (int)right_number_fieldBox.value;
        gate.gate_R.SetFormula(gate.gate_R.calculateMode, gate.gate_R.number);
    }
}
