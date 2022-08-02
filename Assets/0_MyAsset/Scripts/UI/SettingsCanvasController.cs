using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsCanvasController : MonoBehaviour
{
    #region Player
    [Header("Player")]
    [SerializeField] FieldBox player_speed_fieldBox;
    [SerializeField] FieldBox player_forceOfHorizontalMoving_fieldBox;
    [SerializeField] FieldBox player_forceOfGatheringCenter_fieldBox;
    [Space(5)]
    [SerializeField] FieldBox player_startCount_fieldBox;
    [SerializeField] FieldBox player_color_fieldBox;
    [Space(5)]
    [SerializeField] FieldBox player_bloodSplash_fieldBox;
    [SerializeField] FieldBox player_bloodSplash_size_fieldBox;
    [SerializeField] FieldBox player_bloodSplash_speed_fieldBox;
    [Space(5)]
    [SerializeField] FieldBox player_bloodPuddle_fieldBox;
    [SerializeField] FieldBox player_bloodPuddle_size_fieldBox;
    [SerializeField] FieldBox player_bloodPuddle_speed_fieldBox;
    #endregion

    #region Camera
    [Space(20), Header("Camera")]
    [SerializeField] FieldBox camera_positionOffset_fieldBox;
    [SerializeField] FieldBox camera_angle_fieldBox;
    [SerializeField] FieldBox camera_lerpRatio_fieldBox;
    #endregion

    #region Field
    #region Stage
    [Space(20), Header("Field"), Header("Stage")]
    [SerializeField] FieldBox field_stage_length_fieldBox;
    [SerializeField] FieldBox field_stage_width_fieldBox;
    [SerializeField] FieldBox field_stage_centerColor_fieldBox;
    [SerializeField] FieldBox field_stage_edgeColor_fieldBox;
    #endregion

    #region Gates
    [Space(10), Header("Gates")]
    [SerializeField] FieldBox field_gates_gatesCount_fieldBox;
    [SerializeField] ListGroup field_gates_listGroup;
    #endregion

    #region Background

    #endregion

    #region Effects
    #endregion
    #endregion

    #region Effects
    #region Confetti
    [Space(20), Header("Effects")]
    [Header("Confetti")]
    [SerializeField] FieldBox effects_confetti_enabled_fieldBox;
    [SerializeField] FieldBox effects_confetti_size_fieldBox;
    [SerializeField] FieldBox effects_confetti_speed_fieldBox;
    #endregion
    #region Gate
    [Header("Gate")]
    [Header("Light Ring")]
    [SerializeField] FieldBox effects_gate_lightRing_enabled_fieldBox;
    [SerializeField] FieldBox effects_gate_lightRing_size_fieldBox;
    [SerializeField] FieldBox effects_gate_lightRing_speed_fieldBox;
    [Header("Smoke")]
    [SerializeField] FieldBox effects_gate_smoke_enabled_fieldBox;
    [SerializeField] FieldBox effects_gate_smoke_size_fieldBox;
    [SerializeField] FieldBox effects_gate_smoke_speed_fieldBox;
    #endregion
    #region Spark
    [Header("Goal")]
    [Header("Spark")]
    [SerializeField] FieldBox effects_goal_spark_enabled_fieldBox;
    [SerializeField] FieldBox effects_goal_spark_size_fieldBox;
    [SerializeField] FieldBox effects_goal_spark_speed_fieldBox;
    #endregion
    #endregion

    #region System
    [Space(20), Header("System")]
    [SerializeField] FieldBox system_openClearCanvasDelayTime_sec_fieldBox;
    [SerializeField] FieldBox system_openFailCanvasDelayTime_sec_fieldBox;
    #endregion

    [HideInInspector] public bool wasInitialized = false;
    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void Start()
    {
        Initialize();
        EffectViewerManager.i.Initialize_Effects();
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void Initialize()
    {
        #region Player
        PlayerData playerData = DataManager.i.playerData;
        player_speed_fieldBox.SetValue(playerData.speed);
        player_forceOfHorizontalMoving_fieldBox.SetValue(playerData.horizontalPower);
        player_forceOfGatheringCenter_fieldBox.SetValue(playerData.gatherPower);
        player_startCount_fieldBox.SetValue(PlayerManager.i.startCount);
        player_color_fieldBox.SetValue(playerData.color);
        player_bloodSplash_fieldBox.SetValue(playerData.fountainSplash_parameters.useEffect);
        player_bloodSplash_size_fieldBox.SetValue(playerData.fountainSplash_parameters.size);
        player_bloodSplash_speed_fieldBox.SetValue(playerData.fountainSplash_parameters.speed);
        player_bloodPuddle_fieldBox.SetValue(playerData.puddle_parameters.useEffect);
        player_bloodPuddle_size_fieldBox.SetValue(playerData.puddle_parameters.size);
        player_bloodPuddle_speed_fieldBox.SetValue(playerData.puddle_parameters.speed);
        #endregion

        #region Camera
        camera_positionOffset_fieldBox.SetValue(CameraController.i.positionOffset);
        camera_angle_fieldBox.SetValue(CameraController.i.transform.eulerAngles);
        camera_lerpRatio_fieldBox.SetValue(CameraController.i.lerpRatio);
        #endregion

        #region Field
        field_stage_length_fieldBox.SetValue(StageController.i.length);
        field_stage_width_fieldBox.SetValue(StageController.i.width);
        field_stage_centerColor_fieldBox.SetValue(StageController.i.center_color);
        field_stage_edgeColor_fieldBox.SetValue(StageController.i.edge_color);

        field_gates_gatesCount_fieldBox.SetValue(field_gates_listGroup.size);
        #endregion

        #region Effects
        effects_confetti_enabled_fieldBox.SetValue(ConfettiManager.i.effectParameters.useEffect);
        effects_confetti_size_fieldBox.SetValue(ConfettiManager.i.effectParameters.size);
        effects_confetti_speed_fieldBox.SetValue(ConfettiManager.i.effectParameters.speed);
        effects_gate_lightRing_enabled_fieldBox.SetValue(GateMaster.i.lightRing_parameters.useEffect);
        effects_gate_lightRing_size_fieldBox.SetValue(GateMaster.i.lightRing_parameters.size);
        effects_gate_lightRing_speed_fieldBox.SetValue(GateMaster.i.lightRing_parameters.speed);
        effects_gate_smoke_enabled_fieldBox.SetValue(GateMaster.i.smoke_parameters.useEffect);
        effects_gate_smoke_size_fieldBox.SetValue(GateMaster.i.smoke_parameters.size);
        effects_gate_smoke_speed_fieldBox.SetValue(GateMaster.i.smoke_parameters.speed);
        effects_goal_spark_enabled_fieldBox.SetValue(StageController.i.spark_parameters.useEffect);
        effects_goal_spark_size_fieldBox.SetValue(StageController.i.spark_parameters.size);
        effects_goal_spark_speed_fieldBox.SetValue(StageController.i.spark_parameters.speed);
        #endregion

        #region System
        system_openClearCanvasDelayTime_sec_fieldBox.SetValue(GameManager.i.openClearCanvasDelayTime_sec);
        system_openFailCanvasDelayTime_sec_fieldBox.SetValue(GameManager.i.openFailCanvasDelayTime_sec);
        #endregion

        wasInitialized = true;
    }

    public void OnBtnPush_closeSettingsBtn()
    {
        CanvasManager.i.CloseSettingsCanvas();
    }

    #region Player
    public void OnFieldBoxValueChanged_player_speed()
    {
        if (!wasInitialized) return;
        DataManager.i.playerData.speed = (float)player_speed_fieldBox.value;
    }

    public void OnFieldBoxValueChanged_player_forceOfHorizontalMoving()
    {
        if (!wasInitialized) return;
        DataManager.i.playerData.horizontalPower = (float)player_forceOfHorizontalMoving_fieldBox.value;
    }

    public void OnFieldBoxValueChanged_player_forceOfGatheringCenter()
    {
        if (!wasInitialized) return;
        DataManager.i.playerData.gatherPower = (float)player_forceOfGatheringCenter_fieldBox.value;
    }

    public void OnFieldBoxValueChanged_player_startCount()
    {
        if (!wasInitialized) return;
        PlayerManager.i.startCount = (int)player_startCount_fieldBox.value;
    }

    public void OnFieldBoxValueChanged_player_color()
    {
        if (!wasInitialized) return;
        DataManager.i.playerData.color = (Color)player_color_fieldBox.value;
        PlayerManager.i.SetPlayersColor();
    }

    #region BloodSplash
    public void OnFieldBoxValueChanged_player_bloodSplash()
    {
        if (!wasInitialized) return;
        DataManager.i.playerData.fountainSplash_parameters.useEffect = (bool)player_bloodSplash_fieldBox.value;
    }

    public void OnFieldBoxValueChanged_player_bloodSplash_size()
    {
        if (!wasInitialized) return;
        DataManager.i.playerData.fountainSplash_parameters.size = (float)player_bloodSplash_size_fieldBox.value;
    }

    public void OnFieldBoxValueChanged_player_bloodSplash_speed()
    {
        if (!wasInitialized) return;
        DataManager.i.playerData.fountainSplash_parameters.speed = (float)player_bloodSplash_speed_fieldBox.value;
    }
    #endregion

    #region BloodPuddle
    public void OnFieldBoxValueChanged_player_bloodPuddle()
    {
        DataManager.i.playerData.puddle_parameters.useEffect = (bool)player_bloodPuddle_fieldBox.value;
    }

    public void OnFieldBoxValueChanged_player_bloodPuddle_size()
    {
        DataManager.i.playerData.puddle_parameters.size = (float)player_bloodPuddle_size_fieldBox.value;
    }

    public void OnFieldBoxValueChanged_player_bloodPuddle_speed()
    {
        DataManager.i.playerData.puddle_parameters.speed = (float)player_bloodPuddle_speed_fieldBox.value;
    }
    #endregion
    #endregion

    #region Camera
    public void OnFieldBoxValueChanged_camera_positionOffset()
    {
        if (!wasInitialized) return;
        CameraController.i.positionOffset = (Vector3)camera_positionOffset_fieldBox.value;
    }

    public void OnFieldBoxValueChanged_camera_angle()
    {
        if (!wasInitialized) return;
        CameraController.i.transform.eulerAngles = (Vector3)camera_angle_fieldBox.value;
    }

    public void OnFieldBoxValueChanged_camera_lerpRatio()
    {
        if (!wasInitialized) return;
        CameraController.i.lerpRatio = (float)camera_lerpRatio_fieldBox.value;
    }
    #endregion

    #region Field
    #region Stage
    public void OnFieldBoxValueChanged_field_stage_length()
    {
        if (!wasInitialized) return;
        StageController.i.length = (float)field_stage_length_fieldBox.value;
        StageController.i.UpdateStageSize();
    }

    public void OnFieldBoxValueChanged_field_stage_width()
    {
        if (!wasInitialized) return;
        StageController.i.width = (float)field_stage_width_fieldBox.value;
        StageController.i.UpdateStageSize();
    }

    public void OnFieldBoxValueChanged_field_stage_centerColor()
    {
        if (!wasInitialized) return;
        StageController.i.center_color = (Color)field_stage_centerColor_fieldBox.value;
        StageController.i.UpdateStageColor();
    }

    public void OnFieldBoxValueChanged_field_stage_edgeColor()
    {
        if (!wasInitialized) return;
        StageController.i.edge_color = (Color)field_stage_edgeColor_fieldBox.value;
        StageController.i.UpdateStageColor();
    }
    #endregion
    #region Gates
    public void OnFieldBoxValueChanged_field_Gates_GatesCount()
    {
        field_gates_gatesCount_fieldBox.value = Mathf.Clamp((int)field_gates_gatesCount_fieldBox.value, 0, 10);
        field_gates_listGroup.OnSizeChanged((int)field_gates_gatesCount_fieldBox.value);
    }
    #endregion
    #region Background
    #endregion
    #endregion

    #region Effects
    #region Confetti
    public void OnFieldBoxValueChanged_effects_confetti_enabled()
    {
        if (!wasInitialized) return;
        ConfettiManager.i.effectParameters.useEffect = (bool)effects_confetti_enabled_fieldBox.value;
    }

    public void OnFieldBoxValueChanged_effects_confetti_size()
    {
        if (!wasInitialized) return;
        ConfettiManager.i.effectParameters.size = (float)effects_confetti_size_fieldBox.value;
    }

    public void OnFieldBoxValueChanged_effects_confetti_speed()
    {
        if (!wasInitialized) return;
        ConfettiManager.i.effectParameters.speed = (float)effects_confetti_speed_fieldBox.value;
    }
    #endregion
    #region Gate
    public void OnFieldBoxValueChanged_effects_gate_LightRing_enabled()
    {
        if (!wasInitialized) return;
        GateMaster.i.lightRing_parameters.useEffect = (bool)effects_gate_lightRing_enabled_fieldBox.value;
    }

    public void OnFieldBoxValueChanged_effects_gate_LightRing_size()
    {
        if (!wasInitialized) return;
        GateMaster.i.lightRing_parameters.size = (float)effects_gate_lightRing_size_fieldBox.value;
    }

    public void OnFieldBoxValueChanged_effects_gate_LightRing_speed()
    {
        if (!wasInitialized) return;
        GateMaster.i.lightRing_parameters.speed = (float)effects_gate_lightRing_speed_fieldBox.value;
    }
    public void OnFieldBoxValueChanged_effects_gate_smoke_enabled()
    {
        if (!wasInitialized) return;
        GateMaster.i.smoke_parameters.useEffect = (bool)effects_gate_smoke_enabled_fieldBox.value;
    }
    public void OnFieldBoxValueChanged_effects_gate_smoke_size()
    {
        if (!wasInitialized) return;
        GateMaster.i.smoke_parameters.size = (float)effects_gate_smoke_size_fieldBox.value;
    }
    public void OnFieldBoxValueChanged_effects_gate_smoke_speed()
    {
        if (!wasInitialized) return;
        GateMaster.i.smoke_parameters.speed = (float)effects_gate_smoke_speed_fieldBox.value;
    }
    #endregion
    #region Goal
    public void OnFieldBoxValueChanged_effects_goal_spark_enabled()
    {
        if (!wasInitialized) return;
        StageController.i.spark_parameters.useEffect = (bool)effects_goal_spark_enabled_fieldBox.value;
    }
    public void OnFieldBoxValueChanged_effects_goal_spark_size()
    {
        if (!wasInitialized) return;
        StageController.i.spark_parameters.size = (float)effects_goal_spark_size_fieldBox.value;
    }
    public void OnFieldBoxValueChanged_effects_goal_spark_speed()
    {
        if (!wasInitialized) return;
        StageController.i.spark_parameters.speed = (float)effects_goal_spark_speed_fieldBox.value;
    }
    #endregion
    #endregion

    #region Sytem
    public void OnFieldBoxValueChanged_system_openClearCanvasDelayTime_sec()
    {
        if (!wasInitialized) return;
        GameManager.i.openClearCanvasDelayTime_sec = (float)system_openClearCanvasDelayTime_sec_fieldBox.value;
    }

    public void OnFieldBoxValueChanged_system_openFailCanvasDelayTime_sec()
    {
        if (!wasInitialized) return;
        GameManager.i.openFailCanvasDelayTime_sec = (float)system_openFailCanvasDelayTime_sec_fieldBox.value;
    }

    public void OnFieldBoxValueChanged_system_resetSetting()
    {
        //TODO : I'm looking for the best solution of reset settings.
        DataManager.i.playerData.speed = 6;
        DataManager.i.playerData.horizontalPower = 500;
        DataManager.i.playerData.gatherPower = 200;
        DataManager.i.playerData.gatherPower_TowardEnemy = 50;
        DataManager.i.playerData.dieDelayTime_sec = 0.2f;
        DataManager.i.playerData.color = new Color(0, 192f / 255f, 255f / 255f);
        DataManager.i.playerData.fountainSplash_parameters.size = 1;
        DataManager.i.playerData.fountainSplash_parameters.speed = 1;
        DataManager.i.playerData.puddle_parameters.size = 1;
        DataManager.i.playerData.puddle_parameters.speed = 1;

        StageController.i.length = 120;
        StageController.i.width = 6;
        StageController.i.UpdateStageSize();

        Initialize();
    }

    public void OnFieldBoxValueChanged_system_preset01()
    {
        //TODO : I'm looking for the best solution of reset settings.
        DataManager.i.playerData.speed = 9;
        DataManager.i.playerData.horizontalPower = 500;
        DataManager.i.playerData.gatherPower = 200;
        DataManager.i.playerData.gatherPower_TowardEnemy = 50;
        DataManager.i.playerData.dieDelayTime_sec = 0.2f;
        DataManager.i.playerData.color = new Color(0, 255f / 255f, 100f / 255f);
        DataManager.i.playerData.fountainSplash_parameters.size = 1;
        DataManager.i.playerData.fountainSplash_parameters.speed = 1;
        DataManager.i.playerData.puddle_parameters.size = 1;
        DataManager.i.playerData.puddle_parameters.speed = 1;

        StageController.i.length = 120;
        StageController.i.width = 10;
        StageController.i.UpdateStageSize();

        Initialize();
    }

    public void OnFieldBoxValueChanged_system_preset02()
    {
        //TODO : I'm looking for the best solution of reset settings.
        DataManager.i.playerData.speed = 12;
        DataManager.i.playerData.horizontalPower = 500;
        DataManager.i.playerData.gatherPower = 200;
        DataManager.i.playerData.gatherPower_TowardEnemy = 50;
        DataManager.i.playerData.dieDelayTime_sec = 0.2f;
        DataManager.i.playerData.color = new Color(255f / 255f, 0, 0);
        DataManager.i.playerData.fountainSplash_parameters.size = 1.5f;
        DataManager.i.playerData.fountainSplash_parameters.speed = 0.8f;
        DataManager.i.playerData.puddle_parameters.size = 1.5f;
        DataManager.i.playerData.puddle_parameters.speed = 2;

        StageController.i.length = 120;
        StageController.i.width = 4;
        StageController.i.UpdateStageSize();

        Initialize();
    }
    #endregion
}
