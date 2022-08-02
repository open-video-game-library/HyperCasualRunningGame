using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public enum PlayerState
{
    Idle,
    Run,
    Fight,
    Clear,
    WillDie,
    Die
}
public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] List<Renderer> renderers = new List<Renderer>();

    [HideInInspector] public PlayerState playerState = PlayerState.Idle;
    Rigidbody _rigidbody;
    EnemyController couple_enemy;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (animator != null) break;
            Transform _child = transform.GetChild(i);
            if (_child.TryGetComponent(out Animator anim)) animator = anim;
        }

        if (TryGetComponent(out Rigidbody rb)) _rigidbody = rb;
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) OnTouch_Enemy(collision);
    }

    void OnTouch_Enemy(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent(out EnemyController enemy)) return;
        if (playerState == PlayerState.WillDie || playerState == PlayerState.Die) return;
        if (enemy.enemyState == EnemyState.WillDie || enemy.enemyState == EnemyState.Die) return;
        if (enemy.couple_player != null) return;

        playerState = PlayerState.WillDie;
        enemy.enemyState = EnemyState.WillDie;
        couple_enemy = enemy;
        enemy.couple_player = this;
        //StartCoroutine(DelayMethod(DataManager.i.enemyData.dieDelayTime_sec, () => { enemy.Die(); }));
        StartCoroutine(DelayMethod(DataManager.i.playerData.dieDelayTime_sec, () =>
        {
            //TODO: It should be separated from playerCoroutine, but Die(); works before enemy.Die() works, and enemy won't die.
            //->This caused by using ButtonField.
            //->This caused by the difference of dieDelayTime. player dies before enemy dies, and the coroutine didn't work.
            enemy.Die();
            Die();
        }));
    }

    #region OnTouchTrigger
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Gate")) OnTouch_Gate(collider);
        if (collider.CompareTag("Goal")) OnTouch_Goal();
        if (collider.CompareTag("Clear")) OnTouch_Clear();
    }

    void OnTouch_Gate(Collider collider)
    {
        GateController gate = collider.GetComponent<GateController>();
        gate.Use();
    }

    void OnTouch_Goal()
    {
        GameManager.i.BeginGoal();
    }

    void OnTouch_Clear()
    {
        GameManager.i.BeginClear();
    }
    #endregion

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void SetColor(Color c)
    {
        foreach (var _renderer in renderers)
        {
            _renderer.material.color = c;
        }
    }

    public void Initialize(Transform parent, Vector3 pos)
    {
        gameObject.SetActive(true);
        playerState = PlayerState.Run;
        transform.parent = parent;

        Vector3 targetPos = pos + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        if (targetPos.x < -StageController.i.width / 2 + 0.2f) targetPos.x = -StageController.i.width / 2 + 0.2f;
        else if (targetPos.x > StageController.i.width / 2 - 0.2f) targetPos.x = StageController.i.width / 2 - 0.2f;
        transform.position = targetPos;

        SetColor(DataManager.i.playerData.color);
    }

    #region Move
    public void Move()
    {
        if (GameManager.i.gameState == GameState.Ready) return;
        if (GameManager.i.gameState == GameState.Clear) return;
        if (GameManager.i.gameState == GameState.Fail) return;

        Move_Forward();
        Move_Horizontal();
        Move_TowardEnemy();
        Gather();
    }

    void Move_Forward()
    {
        if (playerState == PlayerState.Idle) return;
        if (playerState == PlayerState.Fight) return;
        if (playerState == PlayerState.Die) return;

        _rigidbody.velocity = new Vector3(0, 0, DataManager.i.playerData.speed);
    }

    void Move_TowardEnemy()
    {
        if (playerState == PlayerState.Idle) return;
        if (playerState == PlayerState.Run) return;
        if (playerState == PlayerState.Die) return;

        Vector3 direction = (PlayerManager.i.currentEnemySection.EnemyCenterPos() - transform.position).normalized;
        _rigidbody.AddForce(direction * DataManager.i.playerData.gatherPower_TowardEnemy);
    }

    void Move_Horizontal()
    {
        if (GameManager.i.gameState == GameState.Goal) return;
        if (!CanvasManager.i.inputCanvas.isTouching) return;
        if (playerState == PlayerState.Fight) return;

        Vector3 direction = PlayerManager.i.targetPosition - transform.position;
        if (direction.x > DataManager.i.playerData.horizontalMoveThreshold) _rigidbody.AddForce(new Vector3(DataManager.i.playerData.horizontalPower, 0, 0));
        else if (direction.x < -DataManager.i.playerData.horizontalMoveThreshold) _rigidbody.AddForce(new Vector3(-DataManager.i.playerData.horizontalPower, 0, 0));
        else _rigidbody.AddForce(new Vector3(direction.x * DataManager.i.playerData.horizontalPower, 0, 0));
    }

    void Gather()
    {
        if (playerState == PlayerState.Fight) return;
        Vector3 direction = (PlayerManager.i.PlayerCenterPos() - transform.position).normalized;
        _rigidbody.AddForce(direction * DataManager.i.playerData.gatherPower);
    }
    #endregion

    #region Die
    public void Die(bool isPoolInitialize = false)
    {
        gameObject.SetActive(false);
        PlayerManager.i.players.Remove(this);
        PlayerPoolManager.i.EnqueuePlayer(this);
        playerState = PlayerState.Die;

        if (!isPoolInitialize)
        {
            PlayFountainSplash();
            PlayPuddle();
        }
    }

    void PlayFountainSplash()
    {
        EffectParameters fountainSplash_parameters = DataManager.i.playerData.fountainSplash_parameters;
        if (!fountainSplash_parameters.useEffect) return;

        ParticleSystem fountainSplash;
        if (BloodPoolManager.i.fountainSplash_particleSystem_pool.Count > 0) fountainSplash = BloodPoolManager.i.Dequeue_fountainSplash();
        else fountainSplash = Instantiate(BloodPoolManager.i.fountainSplash_particleSystem_original);
        var fountainSplashMain = fountainSplash.main;
        fountainSplash.transform.localScale = Vector3.one * fountainSplash_parameters.size;
        fountainSplashMain.simulationSpeed = fountainSplash_parameters.speed;
        fountainSplashMain.startColor = DataManager.i.playerData.color;
        fountainSplash.transform.position = transform.position;
        fountainSplash.Play();
    }

    void PlayPuddle()
    {
        EffectParameters puddle_parameters = DataManager.i.playerData.puddle_parameters;
        if (!puddle_parameters.useEffect) return;

        ParticleSystem puddle;
        if (BloodPoolManager.i.puddle_particleSystem_pool.Count > 0) puddle = BloodPoolManager.i.Dequeue_puddle();
        else puddle = Instantiate(BloodPoolManager.i.puddle_particleSystem_original);
        var puddleMain = puddle.main;
        puddle.transform.localScale = Vector3.one * puddle_parameters.size;
        puddleMain.simulationSpeed = puddle_parameters.speed;
        puddleMain.startColor = DataManager.i.playerData.color;
        Vector3 targetPos = transform.position;
        targetPos.y = 0.01f;
        puddle.transform.position = targetPos;
        puddle.Play();
    }
    #endregion

    public void StopRunning_Clear()
    {
        playerState = PlayerState.Clear;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.isKinematic = true;
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    IEnumerator DelayMethod(float delayTime_sec, Action action) { yield return new WaitForSeconds(delayTime_sec); action(); }
}
