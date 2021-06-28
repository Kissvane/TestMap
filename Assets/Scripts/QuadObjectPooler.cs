using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuadObjectPooler : ObjectPooler<QuadBehaviour>
{
    public override void Release(QuadBehaviour go)
    {
        base.Release(go);
        go.gameObject.SetActive(false);
    }
}
