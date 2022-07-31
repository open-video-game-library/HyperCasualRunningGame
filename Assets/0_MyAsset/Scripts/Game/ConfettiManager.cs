using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiManager : MonoBehaviour
{
    public static ConfettiManager i;
    [SerializeField] ParticleSystem confetti_particleSystem;

    public EffectParameters effectParameters;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void Awake()
    {
        i = this;
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void PlayConfetti()
    {
        if (!effectParameters.useEffect) return;

        confetti_particleSystem.transform.localScale = Vector3.one * effectParameters.size;
        var confettiMain = confetti_particleSystem.main;
        confettiMain.simulationSpeed = effectParameters.speed;
        confetti_particleSystem.Play();
    }

}
