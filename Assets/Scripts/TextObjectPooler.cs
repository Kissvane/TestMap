using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextObjectPooler : MonoBehaviour
{
    public Transform NameCanvas;
    public List<Text> pool;
    public GameObject model;

    public Text Pull()
    {
        if (pool.Count > 0)
        {
            Text result = pool[0];
            pool.RemoveAt(0);
            return result;
        }
        else
        {
            GameObject go = Instantiate(model, NameCanvas);
            Text result = go.GetComponent<Text>();
            return result;
        }
    }

    public void Release(Text go)
    {
        pool.Add(go);
        go.gameObject.SetActive(false);
    }
}
