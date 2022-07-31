using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoolManager : MonoBehaviour
{
    public static PlayerPoolManager i;
    [HideInInspector] public List<PlayerController> playerPool;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void Awake()
    {
        i = this;
    }

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.TryGetComponent(out PlayerController player))
            {
                player.Die(true);
            }
        }
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void EnqueuePlayer(PlayerController player)
    {
        player.transform.parent = transform;
        playerPool.Add(player);
    }

    public PlayerController DequeuePlayer()
    {
        PlayerController player = playerPool[0];
        player.gameObject.SetActive(true);
        playerPool.RemoveAt(0);
        return player;
    }
}
