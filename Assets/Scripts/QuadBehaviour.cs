using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Manage quad visual aspect (scale, position, color ...) and behaviour
/// </summary>
public class QuadBehaviour : AbstractQuad
{
    #region variables
    [SerializeField]
    Transform _transform;
    public Transform Transform { get => _transform; private set => _transform = value; }
    [SerializeField]
    Text _text;
    public Text Text { get => _text; set => _text = value; }

    [SerializeField]
    SpriteRenderer _renderer;
    public SpriteRenderer Renderer { get => _renderer; private set => _renderer = value; }
    
    [SerializeField]
    QuadData quadData;
    public QuadData QuadData { get => quadData; private set => quadData = value; }

    [SerializeField]
    Material GlowMaterial;
    [SerializeField]
    Material Default;

    [SerializeField]
    Sprite BorderSprite;
    [SerializeField]
    Sprite BorderlesSprite;

    bool isShaking = false;
    #endregion

    #region text management
    void OnBecameVisible()
    {
        //show the name of the quad if his zoom level is the application zoom level
        if (GameManager.instance.MapZoomer.ZoomLevel == quadData.Level)
        {
            GetFromPool();
        }
    }

    void OnBecameInvisible()
    {
        GiveToPool();
    }

    void OnDisable()
    {
        GiveToPool();
        //if the quad is shaking stop it
        if (isShaking)
        {
            DOTween.Kill(Transform);
            ResetQuadPosition();
        }
    }

    public void DisableText()
    {
        GiveToPool();
    }

    public void EnableTextIfAlreadyVisible()
    {
        if (Renderer.isVisible && GameManager.instance.MapZoomer.ZoomLevel == quadData.Level)
        {
            GetFromPool();
        }
    }

    /// <summary>
    /// get a text from the pool and apply the settings on it to make it look like this quad name's
    /// </summary>
    public void GetFromPool()
    {
        //Get Text from object pooler
        Text = GameManager.instance.TextPool.Pull();
        //set the wanted scale and position
        Transform textTransform = Text.transform;
        textTransform.localPosition = quadData.TextPosition;
        textTransform.localScale = quadData.TextScale;
        Text.text = quadData.Name;
        Text.gameObject.SetActive(true);
    }

    /// <summary>
    /// Release this quad name to the pool
    /// </summary>
    public void GiveToPool()
    {
        if (Text != null) 
        {
            if (isShaking)
            {
                DOTween.Kill(Text.rectTransform);
            }
            //release the text to the object pooler
            GameManager.instance.TextPool.Release(Text);
            //disable it
            Text = null;
        }
    }
    #endregion

    /// <summary>
    /// Manage the click feedback
    /// </summary>
    public void ClickFeedback()
    {
        //stop previous shaking if they exists
        DOTween.Kill(Transform);
        RectTransform textTransform = Text.rectTransform;
        DOTween.Kill(textTransform.transform);
        //reset quad and text position to prevent displacement when clicking multiple times
        ResetQuadPosition();
        ResetTextPosition();
        isShaking = true;
        //shake the Quad and his name
        textTransform.DOShakeAnchorPos(0.5f, quadData.TextShakeStrength * Transform.lossyScale.x).OnComplete(ResetTextPosition);
        Transform.DOShakePosition(0.5f, Transform.lossyScale.x * quadData.QuadShakeStrength, 20, 0).OnComplete(ResetQuadPosition);
    }

    void ResetTextPosition()
    {
        isShaking = false;
        Text.transform.localPosition = quadData.TextPosition;
    }

    void ResetQuadPosition()
    {
        Transform.localPosition = quadData.Position;
    }

    /// <summary>
    /// Use QuadData datas to place, scale and modify visual of this gameObject
    /// </summary>
    /// <param name="data">used QuadData</param>
    public void UseDatas(QuadData data)
    {
        QuadData = data;
        gameObject.SetActive(true);
        Transform.localPosition = quadData.Position;
        Transform.localScale = quadData.Scale;
        Renderer.sortingOrder = quadData.Level - 1;
        Renderer.sprite = quadData.Borders ? BorderSprite : BorderlesSprite;
        Renderer.color = QuadData.Color;
        if (quadData.Glow)
        {
            Renderer.material = GlowMaterial;
        }
        if (GameManager.instance.MapZoomer.ZoomLevel == quadData.Level)
        {
            GetFromPool();
        }
        else
        {
            DisableText();
        }
    }
}
