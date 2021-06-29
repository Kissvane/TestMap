using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Count the quads visible on the screen
/// </summary>
public class QuadCounter : MonoBehaviour
{
    #region variables
    [SerializeField]
    Text Text;

    [SerializeField]
    RectTransform Popup;

    [SerializeField]
    float MoveSpeed = 0.5f;

    [SerializeField]
    Ease Ease;
    #endregion

    /// <summary>
    /// Count the quads visible on screen and show the result
    /// </summary>
    public void ShowResult()
    {
        int visible = 0;
        foreach ( HashSet<QuadData> hashset in GameManager.instance.MapData.Levels.Values)
        {
            foreach (QuadData quad in hashset) 
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

    /// <summary>
    /// animation when the count UI appear
    /// </summary>
    public void PopIn()
    {
        Popup.DOAnchorPos3DX(0f, MoveSpeed).SetEase(Ease).SetSpeedBased();
    }

    /// <summary>
    /// animation when the count UI disappear
    /// </summary>
    public void PopoOut()
    {
        Popup.DOAnchorPos3DX(-Popup.sizeDelta.x, MoveSpeed).SetEase(Ease).SetSpeedBased();
    }
}
