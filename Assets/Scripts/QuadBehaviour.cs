using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuadBehaviour : AbstractQuad
{
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

    public void DisableText()
    {
        GiveToPool();
    }

    public void EnableTextIfAlreadyVisible()
    {
        if (Renderer.isVisible && Linker.instance.MapZoomer.ZoomLevel == quadData.Level)
        {
            GetFromPool();
        }
    }

    public void GetFromPool()
    {
        //Get Text from object pooler
        Text = Linker.instance.TextPool.Pull();
        //set the wanted scale and position
        Transform textTransform = Text.transform;
        textTransform.localPosition = quadData.TextPosition;
        textTransform.localScale = quadData.TextScale;
        Text.text = quadData.Name;
        Text.gameObject.SetActive(true);
    }

    public void GiveToPool()
    {
        if (Text != null) 
        {
            if (isShaking)
            {
                DOTween.Kill(Text.rectTransform);
            }
            //release the text to the object pooler
            Linker.instance.TextPool.Release(Text);
            //disable it
            Text = null;
        }
    }

    public void ClickFeedback()
    {
        DOTween.Kill(Transform);
        RectTransform textTransform = Text.rectTransform;
        DOTween.Kill(textTransform.transform);
        ResetQuadPosition();
        ResetTextPosition();
        isShaking = true;
        //shake the Quad
        textTransform.DOShakeAnchorPos(0.5f, quadData.TextShakeStrength * Transform.lossyScale.x).OnComplete(ResetTextPosition);
        Transform.DOShakePosition(0.5f, Transform.lossyScale.x * quadData.QuadShakeStrength, 20, 0).OnComplete(ResetQuadPosition);
        Linker.instance.QuadCounter.ShowResult();
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
    /// Use quad data to place and scale this gameObject
    /// </summary>
    /// <param name="data"></param>
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
        if (Linker.instance.MapZoomer.ZoomLevel == quadData.Level)
        {
            GetFromPool();
        }
        else
        {
            DisableText();
        }
    }

    public void ClearData()
    {
        QuadData = null;
        Renderer.material = Default;
        GiveToPool();
        gameObject.SetActive(false);
    }
}
