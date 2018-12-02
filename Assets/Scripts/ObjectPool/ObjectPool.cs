using System;
using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    private Dictionary<string, Pool> m_ObjectPools = new Dictionary<string, Pool>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public Pool CreatePool(GameObject obj, int amountToPreSpawn, string uniqueIdentifier)
    {
        try
        {
            if (m_ObjectPools.ContainsKey(uniqueIdentifier))
                return m_ObjectPools[uniqueIdentifier];

            GameObject newPoolObj = new GameObject("[ObjectPool] " + uniqueIdentifier, typeof(Pool));
            newPoolObj.transform.SetParent(transform);
            newPoolObj.transform.localPosition = Vector3.zero;
            Pool pool = newPoolObj.GetComponent<Pool>();

            pool.Initialize(obj, amountToPreSpawn);
            return pool;
        }
        catch(Exception e)
        {
            Debug.LogException(e);
            return null;
        }
    }
}
