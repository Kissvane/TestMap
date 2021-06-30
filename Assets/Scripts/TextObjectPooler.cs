using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manage object pooling for the text used by quad to show their names
/// </summary>
public class TextObjectPooler : MonoBehaviour
{
    public Transform NameCanvas;
    public List<Text> pool;
    public GameObject model;

    /// <summary>
    /// Give an existing or created Text
    /// </summary>
    /// <returns> the text available to show the name </returns>
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

    /// <summary>
    /// Put back a text in the pool
    /// </summary>
    /// <param name="text"> the text to put back </param>
    public void Release(Text text)
    {
        pool.Add(text);
        text.gameObject.SetActive(false);
    }
}
