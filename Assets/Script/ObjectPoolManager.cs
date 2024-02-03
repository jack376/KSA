using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    public Dictionary<string, ObjectPool<GameObject>> pools = new();

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
    }

    private void InitPool(GameObject[] prefabs, int initialSize = 1000)
    {
        foreach (var prefab in prefabs)
        {
            CreatePool(prefab.name, prefab, initialSize);
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
            Debug.LogWarning(key + " no prefab");
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
            Debug.LogWarning(go.name + " no pool");
        }
    }
}