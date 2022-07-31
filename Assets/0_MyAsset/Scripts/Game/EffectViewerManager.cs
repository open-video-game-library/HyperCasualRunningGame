using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectViewerManager : MonoBehaviour
{
    public static EffectViewerManager i;

    [Header("General")]
    public float loopTime_sec = 2;

    [Space(20)]
    [Header("Blood Splash")]
    public Camera bloodSplashCamera;
    public ParticleSystem bloodSplash_particleSystem;

    [Space(20)]
    [Header("Blood Puddle")]
    public Camera bloodPuddleCamera;
    public ParticleSystem bloodPuddle_particleSystem;

    [Space(20)]
    [Header("Gate LightRing")]
    public Camera gateLightRingCamera;
    public ParticleSystem gateLightRing_particleSystem;


    [Space(20)]
    [Header("Gate Smoke")]
    public Camera gateSmokeCamera;
    public ParticleSystem gateSmoke_particleSystem;

    [Space(20)]
    [Header("Goal Spark")]
    public Camera goalSparkCamera;
    public List<ParticleSystem> goalSpark_particleSystem;

    [Space(20)]
    [Header("Clear Confetti")]
    public Camera clearConfettiCamera;
    public ParticleSystem clearConfetti_particleSystem;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void Awake()
    {
        i = this;
    }

    void Start()
    {
        CheckParticleSystemLooping();
    }
    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void Initialize_Effects()
    {
        ChangeSize_bloodSplash();
        ChangeSpeed_bloodSplash();
        ChangeSize_bloodPuddle();
        ChangeSpeed_bloodPuddle();

        ChangeSize_gateLightRing();
        ChangeSpeed_gateLightRing();
        ChangeSize_gateSmoke();
        ChangeSpeed_gateSmoke();

        ChangeSize_goalSpark();
        ChangeSpeed_goalSpark();

        ChangeSize_clearConfetti();
        ChangeSpeed_clearConfetti();
    }

    public void SetViewersActive(bool isActive)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(isActive);
        }
    }

    void CheckParticleSystemLooping()
    {
        if (!bloodSplash_particleSystem.main.loop) StartCoroutine(LoopPlay(bloodSplash_particleSystem));
        else bloodSplash_particleSystem.Play();

        if (!bloodPuddle_particleSystem.main.loop) StartCoroutine(LoopPlay(bloodPuddle_particleSystem));
        else bloodPuddle_particleSystem.Play();

        if (!gateLightRing_particleSystem.main.loop) StartCoroutine(LoopPlay(gateLightRing_particleSystem));
        else gateLightRing_particleSystem.Play();

        if (!gateSmoke_particleSystem.main.loop) StartCoroutine(LoopPlay(gateSmoke_particleSystem));
        else gateSmoke_particleSystem.Play();

        foreach (var spark in goalSpark_particleSystem)
        {
            if (!spark.main.loop) StartCoroutine(LoopPlay(spark));
            else spark.Play();
        }

        if (!clearConfetti_particleSystem.main.loop) StartCoroutine(LoopPlay(clearConfetti_particleSystem, 3));
        else clearConfetti_particleSystem.Play();
    }

    IEnumerator LoopPlay(ParticleSystem particleSystem, float loopTime_sec = 2)
    {
        while (true)
        {
            particleSystem.Stop();
            particleSystem.Play();
            yield return new WaitForSeconds(loopTime_sec);
        }
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    #region Player
    public void ChangeSize_bloodSplash()
    {
        if (!CanvasManager.i.settingsCanvas.wasInitialized) return;
        bloodSplash_particleSystem.transform.localScale = Vector3.one * DataManager.i.playerData.fountainSplash_parameters.size;
    }

    public void ChangeSpeed_bloodSplash()
    {
        if (!CanvasManager.i.settingsCanvas.wasInitialized) return;
        var bloodSplash_main = bloodSplash_particleSystem.main;
        bloodSplash_main.simulationSpeed = DataManager.i.playerData.fountainSplash_parameters.speed;
    }

    public void ChangeSize_bloodPuddle()
    {
        if (!CanvasManager.i.settingsCanvas.wasInitialized) return;
        bloodPuddle_particleSystem.transform.localScale = Vector3.one * DataManager.i.playerData.puddle_parameters.size;
    }

    public void ChangeSpeed_bloodPuddle()
    {
        if (!CanvasManager.i.settingsCanvas.wasInitialized) return;
        var bloodPuddle_main = bloodPuddle_particleSystem.main;
        bloodPuddle_main.simulationSpeed = DataManager.i.playerData.puddle_parameters.speed;
    }
    #endregion

    #region Gate
    public void ChangeSize_gateLightRing()
    {
        if (!CanvasManager.i.settingsCanvas.wasInitialized) return;
        gateLightRing_particleSystem.transform.localScale = Vector3.one * GateMaster.i.lightRing_parameters.size;
    }

    public void ChangeSpeed_gateLightRing()
    {
        if (!CanvasManager.i.settingsCanvas.wasInitialized) return;
        var gateLightRing_main = gateLightRing_particleSystem.main;
        gateLightRing_main.simulationSpeed = GateMaster.i.lightRing_parameters.speed;
    }

    public void ChangeSize_gateSmoke()
    {
        if (!CanvasManager.i.settingsCanvas.wasInitialized) return;
        gateSmoke_particleSystem.transform.localScale = Vector3.one * GateMaster.i.smoke_parameters.size;
    }

    public void ChangeSpeed_gateSmoke()
    {
        if (!CanvasManager.i.settingsCanvas.wasInitialized) return;
        var gateSmoke_main = gateSmoke_particleSystem.main;
        gateSmoke_main.simulationSpeed = GateMaster.i.smoke_parameters.speed;
    }
    #endregion

    #region Goal
    public void ChangeSize_goalSpark()
    {
        if (!CanvasManager.i.settingsCanvas.wasInitialized) return;
        foreach (var spark in goalSpark_particleSystem)
        {
            spark.transform.localScale = Vector3.one * StageController.i.spark_parameters.size;
        }
    }

    public void ChangeSpeed_goalSpark()
    {
        if (!CanvasManager.i.settingsCanvas.wasInitialized) return;
        foreach (var spark in goalSpark_particleSystem)
        {
            var spark_main = spark.main;
            spark_main.simulationSpeed = StageController.i.spark_parameters.speed;
        }
    }
    #endregion

    #region Clear
    public void ChangeSize_clearConfetti()
    {
        if (!CanvasManager.i.settingsCanvas.wasInitialized) return;
        clearConfetti_particleSystem.transform.localScale = Vector3.one * ConfettiManager.i.effectParameters.size;
    }

    public void ChangeSpeed_clearConfetti()
    {
        if (!CanvasManager.i.settingsCanvas.wasInitialized) return;
        var clearConfetti_main = clearConfetti_particleSystem.main;
        clearConfetti_main.simulationSpeed = ConfettiManager.i.effectParameters.speed;
    }
    #endregion
}
