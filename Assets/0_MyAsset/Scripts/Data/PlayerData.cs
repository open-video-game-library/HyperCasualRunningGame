using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("General")]
    public float speed = 1;
    public float horizontalPower = 1000;
    public float gatherPower = 500;
    public float gatherPower_TowardEnemy = 300;
    public float dieDelayTime_sec = 0.1f;
    public Color color = new Color(0 / 255f, 192 / 255f, 255f / 255f);
    public EffectParameters fountainSplash_parameters;
    public EffectParameters puddle_parameters;

    [Space(20)]
    [Header("Advanced")]
    public float horizontalMoveThreshold = 0.5f;
    public List<GameObject> models = new List<GameObject>();
}
