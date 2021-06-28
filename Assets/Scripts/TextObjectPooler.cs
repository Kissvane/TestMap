﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextObjectPooler : ObjectPooler<Text>
{
    public override void Release(Text go)
    {
        base.Release(go);
        go.gameObject.SetActive(false);
    }
}
