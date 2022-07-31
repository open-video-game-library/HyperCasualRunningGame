using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MU5Attribute;

public class MU5AttributeSample : MonoBehaviour
{
    public bool showIfCondition = true;
    [MU5Attribute.ShowIf(nameof(showIfCondition))] public string showIfTest = "aaa";

    [Space(20)]
    public bool hideIfCondition = false;
    [MU5Attribute.HideIf(nameof(hideIfCondition))] public string hideIfTest = "bbb";

    [Space(20)]
    [MU5Attribute.MinMaxSlider(0, 1)] public Vector2 minMaxTestSlider = new Vector2(0.2f, 0.8f);
    [Serializable]
    public class MinMaxTest
    {
        [MU5Attribute.MinMaxSlider(0, 1)] public Vector2 test = new Vector2(0.2f, 0.8f);
        public MinMaxTest2 minMaxTest2;
    }
    [Serializable]
    public class MinMaxTest2
    {
        [MU5Attribute.MinMaxSlider(0, 1)] public Vector2 test2 = new Vector2(0.2f, 0.8f);
    }
    public MinMaxTest minMaxTest;


}
