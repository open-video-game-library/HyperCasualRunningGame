using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public enum EnemyState
{
    Idle,
    Fight,
    WillDie,
    Die
}
public class EnemyController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] List<Renderer> renderers = new List<Renderer>();

    [HideInInspector] public EnemyState enemyState = EnemyState.Idle;
    [HideInInspector] public PlayerController couple_player;
    Rigidbody _rigidbody;
    EnemyManager manager;

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

    void FixedUpdate()
    {
        Move();
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void SetColor(Color c)
    {
        foreach (var _renderer in renderers)
        {
            _renderer.material.color = c;
        }
    }

    public void Initialize(Transform parent, Vector3 pos, EnemyManager _manager)
    {
        gameObject.SetActive(true);
        enemyState = EnemyState.Idle;
        transform.parent = parent;

        Vector3 targetPos = pos + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        if (targetPos.x < -StageController.i.width / 2 + 0.2f) targetPos.x = -StageController.i.width / 2 + 0.2f;
        else if (targetPos.x > StageController.i.width / 2 - 0.2f) targetPos.x = StageController.i.width / 2 - 0.2f;
        transform.position = targetPos;

        SetColor(DataManager.i.enemyData.color);

        manager = _manager;
    }

    void Move()
    {
        _rigidbody.velocity = Vector3.zero;
        Gather();
        Run_TowardPlayer();
    }

    void Gather()
    {
        Vector3 direction = manager.EnemyCenterPos() - transform.position;
        direction.y = 0;
        _rigidbody.AddForce(direction * DataManager.i.enemyData.gatherPower);
    }

    void Run_TowardPlayer()
    {
        if (GameManager.i.gameState != GameState.Play) return;
        if (enemyState != EnemyState.Fight) return;

        Vector3 direction = PlayerManager.i.PlayerCenterPos() - transform.position;
        direction.y = 0;
        _rigidbody.AddForce(direction * DataManager.i.enemyData.gatherPower_TowardPlayer);
    }

    public void Die()
    {
        enemyState = EnemyState.Die;
        manager.enemies.Remove(this);
        manager.CheckPass();
        gameObject.SetActive(false);

        PlayFountainSplash();
        PlayPuddle();
    }

    void PlayFountainSplash()
    {
        EffectParameters fountainSplash_parameters = DataManager.i.enemyData.fountainSplash_parameters;
        if (!fountainSplash_parameters.useEffect) return;

        ParticleSystem fountainSplash;
        if (BloodPoolManager.i.fountainSplash_particleSystem_pool.Count > 0) fountainSplash = BloodPoolManager.i.Dequeue_fountainSplash();
        else fountainSplash = Instantiate(BloodPoolManager.i.fountainSplash_particleSystem_original);
        var fountainSplashMain = fountainSplash.main;
        fountainSplash.transform.localScale = Vector3.one * fountainSplash_parameters.size;
        fountainSplashMain.simulationSpeed = fountainSplash_parameters.speed;
        fountainSplashMain.startColor = DataManager.i.enemyData.color;
        fountainSplash.transform.position = transform.position;
        fountainSplash.Play();
    }

    void PlayPuddle()
    {
        EffectParameters puddle_parameters = DataManager.i.enemyData.puddle_parameters;
        if (!puddle_parameters.useEffect) return;

        ParticleSystem puddle;
        if (BloodPoolManager.i.puddle_particleSystem_pool.Count > 0) puddle = BloodPoolManager.i.Dequeue_puddle();
        else puddle = Instantiate(BloodPoolManager.i.puddle_particleSystem_original);
        var puddleMain = puddle.main;
        puddle.transform.localScale = Vector3.one * puddle_parameters.size;
        puddleMain.simulationSpeed = puddle_parameters.speed;
        puddleMain.startColor = DataManager.i.enemyData.color;
        Vector3 targetPos = transform.position;
        targetPos.y = 0.01f;
        puddle.transform.position = targetPos;
        puddle.Play();
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    IEnumerator DelayMethod(float delayTime_sec, Action action) { yield return new WaitForSeconds(delayTime_sec); action(); }
}
