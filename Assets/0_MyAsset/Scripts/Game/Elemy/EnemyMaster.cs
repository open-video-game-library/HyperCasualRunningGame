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

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out EnemyManager enemyManager))
            {
                enemyManagers.Add(enemyManager);
            }
        }
    }
}
