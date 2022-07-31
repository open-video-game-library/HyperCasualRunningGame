using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateMaster : MonoBehaviour
{
    public static GateMaster i;
    public List<GateManager> gates;

    [Space(20)]
    public EffectParameters lightRing_parameters;
    public EffectParameters smoke_parameters;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void Awake()
    {
        i = this;
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void SetGatesWidth(float num)
    {
        foreach (var gate in gates)
        {
            gate.SetWidth(num);
        }
    }
}
