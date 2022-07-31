using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("General")]
    public float gatherPower = 500;
    public float gatherPower_TowardPlayer = 300;
    public float dieDelayTime_sec = 0.1f;
    public Color color = new Color(1, 0, 0);
    public EffectParameters fountainSplash_parameters;
    public EffectParameters puddle_parameters;

    [Space(20)]
    [Header("Advanced")]
    public List<GameObject> models = new List<GameObject>();
}
