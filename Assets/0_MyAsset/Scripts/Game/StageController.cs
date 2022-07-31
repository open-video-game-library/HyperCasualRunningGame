using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageController : MonoBehaviour
{
    public static StageController i;

    public float length = 150;
    public float width = 6;

    [Space(20)]
    [SerializeField] Transform floor_transform;
    [SerializeField] Transform floorEdge_L_tranform;
    [SerializeField] Transform floorEdge_R_tranform;

    [Space(10)]
    [SerializeField] Transform goal_transform;
    public Transform cameraGoalAnchor_transform;
    [SerializeField] Transform goalBar_L_transform;
    [SerializeField] Transform goalBar_R_transform;
    [SerializeField] SpriteRenderer goalLine_spriteRenderer;
    [SerializeField] SpriteRenderer goalFlag_spriteRenderer;

    [Space(20)]
    [SerializeField] Renderer center_renderer;
    [SerializeField] Renderer edge_L_renderer;
    [SerializeField] Renderer edge_R_renderer;
    [SerializeField] Renderer goalBar_L_renderer;
    [SerializeField] Renderer goalBar_R_renderer;

    [Space(20)]
    public List<ParticleSystem> sparks_particleSystem;

    [Space(20)]
    [SerializeField] GateMaster gateMaster;

    [HideInInspector] public Color center_color;
    [HideInInspector] public Color edge_color;

    [Space(20)]
    public EffectParameters spark_parameters;

    [Space(20)]
    public AnimationCurve fadeoutGoalFlag_animationCurve;
    public float fadeoutCompleteTime_sec = 0.6f;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void Awake()
    {
        i = this;
        center_color = center_renderer.material.color;
        edge_color = edge_L_renderer.material.color;
    }

    void Start()
    {
        center_color = center_renderer.material.color;
        edge_color = edge_L_renderer.material.color;

        cameraGoalAnchor_transform.gameObject.SetActive(false);
    }

    void OnValidate()
    {
        UpdateStageSize();
    }
    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void UpdateStageSize()
    {
        floor_transform.localScale = new Vector3(width, 1, length);
        floorEdge_L_tranform.localScale = new Vector3(1, 1, length);
        floorEdge_L_tranform.localPosition = new Vector3(-width / 2, 0, 0);
        floorEdge_R_tranform.localScale = new Vector3(1, 1, length);
        floorEdge_R_tranform.localPosition = new Vector3(width / 2, 0, 0);

        goal_transform.localPosition = new Vector3(0, 0, length);
        goalBar_L_transform.localPosition = new Vector3(-width / 2 + 0.1f, 2.5f, 0);
        goalBar_R_transform.localPosition = new Vector3(width / 2 - 0.1f, 2.5f, 0);
        UpdateStageSize_GoalLines();

        if (gateMaster != null) gateMaster.SetGatesWidth(width);
    }
    void UpdateStageSize_GoalLines()
    {
        float targetWidth = width - 0.4f;
        targetWidth /= goalLine_spriteRenderer.transform.localScale.x;
        goalLine_spriteRenderer.size = new Vector2(targetWidth, 1);
        goalFlag_spriteRenderer.size = new Vector2(targetWidth, 1);
    }

    public void UpdateStageColor()
    {
        center_renderer.material.color = center_color;
        edge_L_renderer.material.color = edge_color;
        edge_R_renderer.material.color = edge_color;
    }

    public void PlaySparks()
    {
        if (!spark_parameters.useEffect) return;

        foreach (var spark in sparks_particleSystem)
        {
            spark.transform.localScale = Vector3.one * spark_parameters.size;
            var sparkMain = spark.main;
            sparkMain.simulationSpeed = spark_parameters.speed;
            spark.Play();
        }
    }

    public void FadeoutGoalFlag()
    {
        StartCoroutine(FadeoutGoalFlag_coroutine());
    }

    IEnumerator FadeoutGoalFlag_coroutine()
    {
        float time = 0;
        Color startColor_bar = goalBar_L_renderer.material.color;
        Color startColor_flag = goalFlag_spriteRenderer.color;
        Color endColor_bar = startColor_bar;
        Color endColor_flag = startColor_flag;
        endColor_bar.a = 0;
        endColor_flag.a = 0;

        while (time <= fadeoutCompleteTime_sec)
        {
            Color targetColor_bar = Color.Lerp(startColor_bar, endColor_bar, fadeoutGoalFlag_animationCurve.Evaluate(time / fadeoutCompleteTime_sec));
            Color targetColor_flag = Color.Lerp(startColor_flag, endColor_flag, fadeoutGoalFlag_animationCurve.Evaluate(time / fadeoutCompleteTime_sec));
            goalBar_L_renderer.material.color = targetColor_bar;
            goalBar_R_renderer.material.color = targetColor_bar;
            goalFlag_spriteRenderer.color = targetColor_flag;

            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        goalBar_L_renderer.material.color = endColor_bar;
        goalBar_R_renderer.material.color = endColor_bar;
        goalFlag_spriteRenderer.color = endColor_flag;
    }
}
