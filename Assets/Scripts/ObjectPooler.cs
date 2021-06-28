using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPooler<T> : MonoBehaviour
{
    public Transform PoolParentTransform;
    public List<T> pool;
    public GameObject model;

    public T Pull()
    {
        if (pool.Count > 0)
        {
            T result = pool[0];
            pool.RemoveAt(0);
            return result;
        }
        else
        {
            GameObject go = Instantiate(model, PoolParentTransform);
            T result = go.GetComponent<T>();
            return result;
        }
    }

    public virtual void Release(T go)
    {
        pool.Add(go);
    }
}
