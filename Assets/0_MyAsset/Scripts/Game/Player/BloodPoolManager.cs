using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPoolManager : MonoBehaviour
{
    public static BloodPoolManager i;

    [Header("FountainSplash")]
    public ParticleSystem fountainSplash_particleSystem_original;
    [SerializeField] Transform fountainSplash_parent;
    [HideInInspector] public List<ParticleSystem> fountainSplash_particleSystem_pool = new List<ParticleSystem>();

    [Header("Puddle")]
    [Space(20)]
    public ParticleSystem puddle_particleSystem_original;
    [SerializeField] Transform puddle_parent;
    [HideInInspector] public List<ParticleSystem> puddle_particleSystem_pool = new List<ParticleSystem>();

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void Awake()
    {
        i = this;
    }

    void Start()
    {
        for (int i = 0; i < fountainSplash_parent.childCount; i++)
        {
            Transform child = fountainSplash_parent.GetChild(i);
            if (child.TryGetComponent(out ParticleSystem fountainSplash))
            {
                fountainSplash_particleSystem_pool.Add(fountainSplash);
            }
        }
        for (int i = 0; i < puddle_parent.childCount; i++)
        {
            Transform child = puddle_parent.GetChild(i);
            if (child.TryGetComponent(out ParticleSystem puddle))
            {
                puddle_particleSystem_pool.Add(puddle);
            }
        }
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void Enqueue_fountainSplash(ParticleSystem fountainSplash)
    {
        fountainSplash.transform.parent = fountainSplash_parent;
        fountainSplash_particleSystem_pool.Add(fountainSplash);
    }

    public ParticleSystem Dequeue_fountainSplash()
    {
        ParticleSystem fountainSplash = fountainSplash_particleSystem_pool[0];
        fountainSplash.gameObject.SetActive(true);
        fountainSplash_particleSystem_pool.RemoveAt(0);
        return fountainSplash;
    }

    public void Enqueue_puddle(ParticleSystem puddle)
    {
        puddle.transform.parent = puddle_parent;
        puddle_particleSystem_pool.Add(puddle);
    }

    public ParticleSystem Dequeue_puddle()
    {
        ParticleSystem puddle = puddle_particleSystem_pool[0];
        puddle.gameObject.SetActive(true);
        puddle_particleSystem_pool.RemoveAt(0);
        return puddle;
    }
}
