using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CalculateMode
{
    Plus,
    Minus,
    Multiply,
    Divid
}
public class GateManager : MonoBehaviour
{
    public Material gate_blue_mat;
    public Material gate_red_mat;
    public ParticleSystem lightRing_particleSystem;
    public ParticleSystem smoke_particleSystem;

    [Space(20)]
    [Header("Left")]
    public GateController gate_L;
    public Canvas canvas_L;
    [SerializeField] Transform bar_L_transform;
    [SerializeField] CalculateMode calculateMode_L;
    [SerializeField] int number_L = 10;

    [Space(20)]
    [Header("Right")]
    public GateController gate_R;
    public Canvas canvas_R;
    [SerializeField] Transform bar_R_transform;
    [SerializeField] CalculateMode calculateMode_R;
    [SerializeField] int number_R = 10;

    [HideInInspector] public bool wasUsed = false;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void OnValidate()
    {
        gate_L.SetFormula(calculateMode_L, number_L);
        gate_R.SetFormula(calculateMode_R, number_R);
    }

    public void SetWidth(float num)
    {
        gate_L.transform.localScale = new Vector3(num / 2 - 0.2f, 1, 1);
        gate_R.transform.localScale = new Vector3(num / 2 - 0.2f, 1, 1);
        canvas_L.transform.localPosition = new Vector3(-(num / 2 - 0.2f) / 2, 1, -0.01f);
        canvas_R.transform.localPosition = new Vector3((num / 2 - 0.2f) / 2, 1, -0.01f);
        bar_L_transform.localPosition = new Vector3(-num / 2 + 0.275f, 1, 0);
        bar_R_transform.localPosition = new Vector3(num / 2 - 0.275f, 1, 0);
    }
}
