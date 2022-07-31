using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemyController enemy_original;
    [SerializeField] GameObject anchorObject;
    [SerializeField] GameObject wall;

    [Space(10)]
    [Min(1)] public int startCount = 1;

    [HideInInspector] public List<EnemyController> enemies = new List<EnemyController>();

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void Start()
    {
        anchorObject.SetActive(false);
        MakeEnemy(startCount);
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player")) OnTouch_Player();
    }

    void OnTouch_Player()
    {
        wall.SetActive(false);
        foreach (var enemy in enemies)
        {
            enemy.enemyState = EnemyState.Fight;
        }
        PlayerManager.i.BeginFighting(this);
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void MakeEnemy(int num)
    {
        for (int i = 0; i < num; i++)
        {
            EnemyController enemy_clone;
            enemy_clone = Instantiate(enemy_original);
            enemy_clone.Initialize(transform, transform.position, this);
            enemies.Add(enemy_clone);
        }
    }

    public Vector3 EnemyCenterPos()
    {
        Vector3 pos = new Vector3();
        foreach (var enemy in enemies)
        {
            pos += enemy.transform.position;
        }
        pos /= enemies.Count;
        return pos;
    }

    public void CheckPass()
    {
        if (enemies.Count > 0) return;

        GameManager.i.gameState = GameState.Play;
        PlayerManager.i.BeginPlayersRunning();
        gameObject.SetActive(false);
    }
}
