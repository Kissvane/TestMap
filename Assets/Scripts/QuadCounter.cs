using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Count the number of visible
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
    /// Count the number of visible and show it
    /// </summary>
    public void ShowResult()
    {
        int visible = 0;
        for (int i = 0; i < GameManager.instance.MapConstructor.Levels.Count; i++)
        {
            foreach (QuadBehaviour quad in GameManager.instance.MapConstructor.Levels[i])
            {
                if (quad.Renderer.isVisible)
                {
                    visible++;
                }
            }
        }

        Text.text = visible.ToString();
        PopIn();
    }

    /// <summary>
    /// Showing animation of quad counter UI
    /// </summary>
    public void PopIn()
    {
        Popup.DOAnchorPos3DX(0f, MoveSpeed).SetEase(Ease).SetSpeedBased();
    }

    /// <summary>
    /// Hiding animation of quad counter UI
    /// </summary>
    public void PopoOut()
    {
        Popup.DOAnchorPos3DX(-Popup.sizeDelta.x, MoveSpeed).SetEase(Ease).SetSpeedBased();
    }
}
