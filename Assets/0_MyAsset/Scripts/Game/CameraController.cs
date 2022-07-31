using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController i;

    public Vector3 positionOffset = new Vector3(0, 7, -7);
    public float lerpRatio = 0.5f;

    [Space(20)]
    public AnimationCurve moveGoalPosition_animationCurve;
    public float moveCompleteTime_sec = 0.6f;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void Awake()
    {
        i = this;
    }

    void LateUpdate()
    {
        Move();
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void Move()
    {
        if (GameManager.i.gameState == GameState.Goal) return;
        if (GameManager.i.gameState == GameState.Clear) return;
        if (GameManager.i.gameState == GameState.Fail) return;

        Vector3 targetPos = Vector3.Lerp(transform.position, PlayerManager.i.PlayerCenterPos() + positionOffset, lerpRatio * Time.deltaTime);
        targetPos.x = 0;
        transform.position = targetPos;
    }

    public void MoveGoalPosition()
    {
        StartCoroutine(GoalMove());
    }

    IEnumerator GoalMove()
    {
        float time = 0;
        Vector3 startPos = transform.position;
        Vector3 endPos = StageController.i.cameraGoalAnchor_transform.position;
        Vector3 startAngle = transform.eulerAngles;
        Vector3 endAngle = StageController.i.cameraGoalAnchor_transform.eulerAngles;

        while (time <= moveCompleteTime_sec)
        {
            Vector3 targetPos = Vector3.Lerp(startPos, endPos, moveGoalPosition_animationCurve.Evaluate(time / moveCompleteTime_sec));
            Vector3 targetAngle = Vector3.Lerp(startAngle, endAngle, moveGoalPosition_animationCurve.Evaluate(time / moveCompleteTime_sec));
            transform.position = targetPos;
            transform.eulerAngles = targetAngle;

            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        transform.position = endPos;
        transform.eulerAngles = endAngle;
    }
}
