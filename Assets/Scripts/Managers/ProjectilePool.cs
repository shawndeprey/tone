using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectilePool : MonoBehaviour
{
    public int poolSize;
    public int poolMax = 0;
    public GameObject goPrefab;
    public static ProjectilePool Instance { get { return _instance; } }

    public List<GameObject> pool;
    private int poolTotal = 0;
    private static ProjectilePool _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }

        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }

        poolMax = poolMax != 0 && poolSize > poolMax ? poolSize : poolMax;
    }

    private void Initialize()
    {
        poolTotal = 0;
        pool = new List<GameObject>();

        // Create initial pool of objects
        AddMoreToPool(poolSize);
    }

    public GameObject Create()
    {
        return Create(Vector3.zero);
    }

    public GameObject Create(Vector3 position)
    {
        GameObject go = GetFromPool(goPrefab);

        if (go == null)
        {
            return null;
        }

        Transform transform = go.transform;
        transform.localPosition = position;

        return go;
    }

    public GameObject Create(GameObject prefab)
    {
        return Create(prefab, Vector3.zero);
    }

    public GameObject Create(GameObject prefab, Vector3 position)
    {
        GameObject go = GetFromPool(prefab);

        Transform transform = go.transform;
        transform.localPosition = position;

        return go;
    }

    public void Recycle(GameObject go)
    {
        if (PutBackInPool(go) == null)
        {
            Destroy(go);
            return;
        }
        go.SetActive(false);
    }

    public void Recycle(GameObject go, float seconds)
    {
        StartCoroutine(RecycleObject(go, seconds));
    }

    private void AddMoreToPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // If poolMax is set to zero or a negative number, then there is no pool limit
            if (poolMax != 0 && poolTotal > poolMax)
            {
                Debug.LogError("ERROR: Maximum pool size for ProjectilePool reached - " + poolMax + ".");
                return;
            }

            GameObject go = (GameObject)Instantiate(goPrefab);
            go.SetActive(false);
            go.transform.SetParent(this.transform);
            pool.Add(go);
            poolTotal++;
        }
    }

    private GameObject AddOneToPool(GameObject prefab)
    {
        // If poolMax is set to zero or a negative number, then there is no pool limit
        if (poolMax != 0 && poolTotal > poolMax)
        {
            Debug.LogError("ERROR: Maximum pool size for ProjectilePool reached - " + poolMax + ".");
            return null;
        }

        GameObject go = (GameObject)Instantiate(prefab);
        go.SetActive(false);
        go.transform.SetParent(this.transform);
        pool.Add(go);
        poolTotal++;

        return go;
    }

    private GameObject PutBackInPool(GameObject go)
    {
        // If poolMax is set to zero or a negative number, then there is no pool limit
        if (poolMax != 0 && pool.Count > poolMax)
        {
            Debug.LogError("ERROR: Maximum pool size for ProjectilePool reached - " + poolMax + ".");
            return null;
        }

        go.SetActive(false);
        pool.Add(go);
        poolTotal++;

        return go;
    }

    IEnumerator RecycleObject(GameObject go, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Recycle(go);
    }

    private GameObject GetFromPool(GameObject prefab)
    {
        GameObject go = null;

        if (pool.Count > 0)
        {
            go = pool[0];
            pool.RemoveAt(0);
        }
        else
        {
            go = AddOneToPool(prefab);
        }

        return go;
    }
}