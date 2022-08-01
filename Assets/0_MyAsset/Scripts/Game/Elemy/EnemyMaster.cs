using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaster : MonoBehaviour
{
    public static EnemyMaster i;

    [HideInInspector] public List<EnemyManager> enemyManagers = new List<EnemyManager>();

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー\
    void Awake()
    {
        i = this;
    }

    void OnValidate()
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out EnemyManager enemyManager))
            {
                enemyManagers.Add(enemyManager);
            }
        }
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void SetEnemyTriggerWidth(float num)
    {
        foreach (var enemyManager in enemyManagers)
        {
            enemyManager.SetWidth(num);
        }
    }
}
