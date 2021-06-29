﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuadCounter : MonoBehaviour
{
    [SerializeField]
    Text Text;

    [SerializeField]
    RectTransform Popup;

    [SerializeField]
    float MoveSpeed = 0.5f;

    [SerializeField]
    Ease Ease;

    public List<QuadData> test;

    public void ShowResult()
    {
        int visible = 0;
        foreach (QuadData[] array in GameManager.instance.MapData.Levels)
        {
            foreach (QuadData quad in array) 
            {
                if (quad.Quad.Renderer.isVisible)
                {
                    visible++;
                }
            }
        }

        Text.text = visible.ToString();
        PopIn();
    }

    public void PopIn()
    {
        Popup.DOAnchorPos3DX(0f, MoveSpeed).SetEase(Ease).SetSpeedBased();
    }

    public void PopoOut()
    {
        Popup.DOAnchorPos3DX(-Popup.sizeDelta.x, MoveSpeed).SetEase(Ease).SetSpeedBased();
    }
}
