using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public Dictionary<string, ObjectPool<GameObject>> pools = new();

    public static ObjectPoolManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitPool(Resources.LoadAll<GameObject>("Particle/"));
        InitPool(Resources.LoadAll<GameObject>("DamageText/"));
    }

    private void InitPool(GameObject[] prefabs, int initialSize = 1000)
    {
        foreach (var prefab in prefabs)
        {
            CreatePool(prefab.name, prefab, initialSize);
            Debug.Log($"Create Pool: {prefab.name}");
        }
    }

    public void CreatePool(string key, GameObject prefab, int initialSize = 1000)
    {
        var createPool = new ObjectPool<GameObject>
        (
            createFunc: () => Instantiate(prefab, transform.position, Quaternion.identity),
            actionOnGet: instance =>
            {
                instance.transform.position = transform.position;
                instance.SetActive(true);
            },
            actionOnRelease: instance =>
            {
                instance.SetActive(false);
            },
            defaultCapacity: initialSize
        );

        pools.Add(key, createPool);
    }

    public ObjectPool<GameObject> GetPool(string key)
    {
        if (!pools.ContainsKey(key))
        {
            Debug.LogWarning($"No pool: {key}");
            return null;
        }

        return pools[key];
    }

    public void ReleasePool(string key, GameObject go)
    {
        if (pools.TryGetValue(key, out var pool))
        {
            pool.Release(go);
        }
        else
        {
            Debug.LogWarning($"No pool: {key}");
        }
    }
}