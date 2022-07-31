using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SafeArea : MonoBehaviour
{
    [SerializeField] RectTransform test;
    [SerializeField] TMP_Text debugTxt;
    RectTransform rectTransform;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        Rect safeArea = Screen.safeArea;
        test.anchorMin = new Vector2(0f, 0f);
        test.anchorMax = new Vector2(0f, 0f);
        test.pivot = new Vector2(0, 0);
        test.sizeDelta = new Vector2(safeArea.width, safeArea.height);
        //test.localPosition = new Vector2(safeArea.x, safeArea.y);
        test.position = new Vector2(safeArea.x, safeArea.y);
        if (debugTxt != null) debugTxt.text = $"(,,{Screen.width},{Screen.height})\n{test.rect}\n{test.localPosition}\n{safeArea}";
    }
}
