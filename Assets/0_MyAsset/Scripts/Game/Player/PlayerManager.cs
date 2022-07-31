using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MU5Attribute;

[Serializable]
public class EffectParameters
{
    public bool useEffect = true;
    [ShowIf(nameof(useEffect))] public float size = 1f;
    [ShowIf(nameof(useEffect))] public float speed = 1f;
}

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager i;

    [SerializeField] PlayerController player_original;
    public List<PlayerController> players = new List<PlayerController>();

    [Space(10)]
    [Min(1)] public int startCount = 1;

    [HideInInspector] public Vector3 targetStartPosition;
    [HideInInspector] public Vector3 targetPosition;
    [HideInInspector] public EnemyManager currentEnemySection;

    Vector3 previousPlayerCenterPos;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    #region General Method
    void Awake()
    {
        i = this;
    }

    void Start()
    {
        SetPlayersColor();
    }

    void Update()
    {
        previousPlayerCenterPos = PlayerCenterPos();
    }

    void FixedUpdate()
    {
        MovePlayers();
    }
    #endregion

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void SetPlayersColor()
    {
        foreach (var player in players)
        {
            player.SetColor(DataManager.i.playerData.color);
        }
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void BeginPlayersRunning()
    {
        foreach (var player in players)
        {
            player.playerState = PlayerState.Run;
        }
    }

    public void StopPlayersRunning_Clear()
    {
        foreach (var player in players)
        {
            player.StopRunning_Clear();
        }
    }

    #region Move
    public void UpdateTargetStartPosition()
    {
        if (players.Count > 0) targetStartPosition = PlayerCenterPos() + new Vector3(StageController.i.width * CanvasManager.i.inputCanvas.moveValue, 0, 0);
    }

    Vector3 GetTargetPosition()
    {
        Vector3 targetPos = new Vector3(0, 0, 0);
        targetPos = targetStartPosition + new Vector3(StageController.i.width * CanvasManager.i.inputCanvas.moveValue, 0, 0);

        if (targetPos.x < -StageController.i.width / 3 + 0.25f) targetPos.x = -StageController.i.width / 2 + 0.25f;
        else if (StageController.i.width / 3 - 0.25f < targetPos.x) targetPos.x = StageController.i.width / 2 - 0.25f;

        return targetPos;
    }

    void MovePlayers()
    {
        targetPosition = GetTargetPosition();
        foreach (var player in players)
        {
            player.Move();
        }
    }
    #endregion

    public void BeginFighting(EnemyManager enemyManager)
    {
        if (GameManager.i.gameState == GameState.Play_inactive) return;
        GameManager.i.gameState = GameState.Play_inactive;

        foreach (var player in players)
        {
            player.playerState = PlayerState.Fight;
        }
        currentEnemySection = enemyManager;
    }

    #region Calculate
    public void AddPlayer(int num)
    {
        for (int i = 0; i < num; i++)
        {
            PlayerController player_clone;
            if (PlayerPoolManager.i.playerPool.Count > 0) player_clone = PlayerPoolManager.i.DequeuePlayer();
            else player_clone = Instantiate(player_original);
            player_clone.Initialize(transform, PlayerCenterPos());
            players.Add(player_clone);
        }
    }

    public void KillPlayer(int num)
    {
        for (int i = 0; i < num; i++)
        {
            if (players.Count == 0) break;
            players[0].Die();
            GameManager.i.CheckFail();
        }
    }
    #endregion

    public Vector3 PlayerCenterPos()
    {
        if (players.Count == 0) return previousPlayerCenterPos;

        Vector3 pos = new Vector3();
        foreach (var player in players)
        {
            pos += player.transform.position;
        }
        pos /= players.Count;
        return pos;
    }
}
