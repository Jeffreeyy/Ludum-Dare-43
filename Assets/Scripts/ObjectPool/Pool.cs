using UnityEngine;
using System.Collections.Generic;

public class PoolObject
{
    public GameObject obj;
    public bool inUse;

    public PoolObject(GameObject obj, bool inUse)
    {
        this.obj = obj;
        this.inUse = inUse;
    }
}

public class Pool : MonoBehaviour
{
    private GameObject m_OriginalObject;
    private List<PoolObject> m_Objects = new List<PoolObject>();

    public void Initialize(GameObject obj, int amountToPreSpawn)
    {
        m_OriginalObject = obj;

        for (int i = 0; i < amountToPreSpawn; i++)
            CreatePoolObject(obj);
    }

    public GameObject Get()
    {
        for (int i = 0; i < m_Objects.Count; i++)
        {
            if(!m_Objects[i].inUse)
            {
                m_Objects[i].inUse = true;
                return m_Objects[i].obj;
            }
        }
        PoolObject newPoolObj = CreatePoolObject(m_OriginalObject);
        newPoolObj.inUse = true;
        return newPoolObj.obj;
    }

    public void Despawn(GameObject obj)
    {
        for (int i = 0; i < m_Objects.Count; i++)
        {
            if(m_Objects[i].obj == obj)
            {
                m_Objects[i].inUse = false;
                m_Objects[i].obj.transform.SetParent(transform);
                m_Objects[i].obj.transform.localPosition = Vector3.zero;
            }
        }
    }

    private PoolObject CreatePoolObject(GameObject obj)
    {
        PoolObject poolObj = new PoolObject(Instantiate(obj, transform, false), false);
        poolObj.obj.transform.SetParent(transform);
        poolObj.obj.transform.localPosition = Vector3.zero;
        m_Objects.Add(poolObj);

        return poolObj;
    }
}