using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool {
    private List<Pool> pools;
    private int poolSizePerVariant;
    private Transform parent;

    public ObjectPool(List<GameObject[]> prefabs, int poolSizePerVariant, Transform parent = null) {
        pools = new List<Pool>();
        this.parent = parent;
        this.poolSizePerVariant = poolSizePerVariant;
        for (int i = 0; i < prefabs.Count; i++) {
            Pool pool = InitPool(prefabs[i]);
            pools.Add(pool);
        }
    }

    public ObjectPool(GameObject[] prefabs, int poolSizePerVariant, Transform parent = null) {
        this.parent = parent;
        this.poolSizePerVariant = poolSizePerVariant;
        pools = new List<Pool>();
        Pool pool = InitPool(prefabs);
        pools.Add(pool);
    }

    private Pool InitPool(GameObject[] prefabs) {
        Pool pool = new Pool();
        for (int variant = 0; variant < prefabs.Length; variant++) {
            var gameObjects = new List<GameObject>();
            for (int j = 0; j < poolSizePerVariant; j++) {
                GameObject gameObject = GameObject.Instantiate(prefabs[variant]);
                if (parent) {
                    gameObject.transform.parent = parent;
                }
                gameObject.SetActive(false);
                pool.AddObject(gameObject, variant);
            }
        }
        return pool;
    }

    public GameObject GetFromPool(int variant) {
        return GetFromPool(0, variant);
    }

    public GameObject GetFromPool(int type, int variant) {
        Pool pool = pools[type];
        List<GameObject> randomObjectVariants = pool.GetObjects(variant);
        if (randomObjectVariants.Count > 0) {
            for (int i = 0; i < poolSizePerVariant; i++) {
                GameObject gameObject = randomObjectVariants[i];
                if (!gameObject.activeSelf) {
                    return gameObject;
                }
            }
        }
        return null;
    }

    public int GetVariantCountFor(int type = 0) {
        return pools[type].GetVariantCount();
    }

    private class Pool {
        Dictionary<int, List<GameObject>> objects;

        public Pool() {
            objects = new Dictionary<int, List<GameObject>>();
        }

        public int GetVariantCount() {
            return objects.Count;
        }

        public void AddObject(GameObject gameObject, int variant) {
            if (!objects.ContainsKey(variant)) {
                objects[variant] = new List<GameObject>();
            }
            objects[variant].Add(gameObject);
        }

        public List<GameObject> GetObjects(int variant) {
            return objects[variant];
        }
    }
}