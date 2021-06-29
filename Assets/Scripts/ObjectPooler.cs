using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Abstract implementation of object pooling
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectPooler<T> : MonoBehaviour
{
    public Transform PoolParentTransform;
    public List<T> pool;
    public GameObject model;

    /// <summary>
    /// Pick one object in the pool
    /// </summary>
    /// <returns> picked object </returns>
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

    /// <summary>
    /// Release an object in the pool
    /// </summary>
    /// <param name="go"> released object </param>
    public virtual void Release(T go)
    {
        pool.Add(go);
    }
}
