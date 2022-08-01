using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCanvasController : MonoBehaviour
{
    [HideInInspector] public bool isTouching = false;

    Vector2 mouseBeginPos;
    [HideInInspector] public float moveValue;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void Update()
    {
        UpdateMoveValue();
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void OnTouchBegin()
    {
        isTouching = true;
        mouseBeginPos = Input.mousePosition;
        moveValue = 0;

        GameManager.i.BeginGame();
        PlayerManager.i.UpdateTargetStartPosition();
    }

    public void OnTouchEnd()
    {
        isTouching = false;
    }

    void UpdateMoveValue()
    {
        if (GameManager.i.gameState == GameState.Play_inactive) return;
        if (!isTouching) return;

        Vector2 mousePos = Input.mousePosition;
        Vector2 mouseMoveDifference = mousePos - mouseBeginPos;
        moveValue = mouseMoveDifference.x / (float)Screen.width;
    }
}
