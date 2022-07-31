using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GateController : MonoBehaviour
{
    public GateManager manager;
    [SerializeField] Renderer _renderer;
    [SerializeField] TMP_Text formula_tmpTxt;

    [HideInInspector] public CalculateMode calculateMode;
    [HideInInspector] public int number;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void SetFormula(CalculateMode mode, int num)
    {
        calculateMode = mode;
        number = num;

        if (mode == CalculateMode.Plus)
        {
            _renderer.material = manager.gate_blue_mat;
            formula_tmpTxt.text = $"+{num}";
        }
        else if (mode == CalculateMode.Minus)
        {
            _renderer.material = manager.gate_red_mat;
            formula_tmpTxt.text = $"-{num}";
        }
        else if (mode == CalculateMode.Multiply)
        {
            _renderer.material = manager.gate_blue_mat;
            formula_tmpTxt.text = $"x{num}";
        }
        else if (mode == CalculateMode.Divid)
        {
            _renderer.material = manager.gate_red_mat;
            formula_tmpTxt.text = $"÷{num}";
        }
    }

    public void Use()
    {
        if (manager.wasUsed) return;

        manager.wasUsed = true;

        if (calculateMode == CalculateMode.Plus) PlayerManager.i.AddPlayer(number);
        else if (calculateMode == CalculateMode.Minus) PlayerManager.i.KillPlayer(number);
        else if (calculateMode == CalculateMode.Multiply) PlayerManager.i.AddPlayer(PlayerManager.i.players.Count * (number - 1));
        else if (calculateMode == CalculateMode.Divid) PlayerManager.i.KillPlayer(PlayerManager.i.players.Count * (number - 1) / number);

        PlayEffect();
    }

    public void PlayEffect()
    {
        if (!GateMaster.i.lightRing_parameters.useEffect) return;

        if (calculateMode == CalculateMode.Plus || calculateMode == CalculateMode.Multiply)
        {
            ParticleSystem lightRing = manager.lightRing_particleSystem;
            if (transform.localEulerAngles.y == 0) lightRing.transform.position = manager.canvas_R.transform.position;
            else lightRing.transform.position = manager.canvas_L.transform.position;
            lightRing.transform.localScale = Vector3.one * GateMaster.i.lightRing_parameters.size;
            var lightRingMain = lightRing.main;
            lightRingMain.simulationSpeed = GateMaster.i.lightRing_parameters.speed;
            lightRing.Play();
        }
        else if (calculateMode == CalculateMode.Minus || calculateMode == CalculateMode.Divid)
        {
            ParticleSystem smoke = manager.smoke_particleSystem;
            if (transform.localEulerAngles.y == 0) smoke.transform.position = manager.canvas_R.transform.position;
            else smoke.transform.position = manager.canvas_L.transform.position;
            smoke.transform.localScale = Vector3.one * GateMaster.i.smoke_parameters.size;
            var smokeMain = smoke.main;
            smokeMain.simulationSpeed = GateMaster.i.smoke_parameters.speed;
            smoke.Play();
        }
    }
}
