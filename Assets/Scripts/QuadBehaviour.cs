using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class JsonQuadData
{
    public float QuadShakeStrength;
    public float TextShakeStrength;
    public string QuadName;
    public Vector3 TextPosition;
    public Vector3 TextScale;
    public Vector3 InitialPosition;
    public Vector3 Scale;
    public Color Color;
    public int Level;
    public bool Glow;

    public JsonQuadData(QuadBehaviour quad)
    {
        QuadShakeStrength = quad.QuadShakeStrength;
        TextShakeStrength = quad.TextShakeStrength;
        QuadName = quad.QuadName;
        TextPosition = quad.TextPosition;
        TextScale = quad.TextScale;
        InitialPosition = quad.InitialPosition;
        Level = quad.Level;
        Color = quad.Renderer.color;
        Scale = quad.Transform.lossyScale;
        Glow = quad.Glow;
    }

    public JsonQuadData()
    {

    }
}

public class QuadBehaviour : MonoBehaviour
{
    [SerializeField]
    Transform _transform;
    public Transform Transform { get => _transform; private set => _transform = value; }

    [SerializeField]
    float quadShakeStrength = 1f;
    public float QuadShakeStrength { get => quadShakeStrength; private set => quadShakeStrength = value; }
    
    [SerializeField]
    float textShakeStrength = 1f;
    public float TextShakeStrength { get => textShakeStrength; private set => textShakeStrength = value; }
    
    [SerializeField]
    Text Text;
    
    [SerializeField]
    string quadName;
    public string QuadName { get => quadName; private set => quadName = value; }
    
    [SerializeField]
    Vector3 textPosition;
    public Vector3 TextPosition { get => textPosition; private set => textPosition = value; }
    
    [SerializeField]
    Vector3 textScale;
    public Vector3 TextScale { get => textScale; private set => textScale = value; }
    
    [SerializeField]
    Vector3 initialPosition;
    public Vector3 InitialPosition { get => initialPosition; private set => initialPosition = value; }

    [SerializeField]
    private int level;
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    private bool glow;
    public bool Glow { get => glow; set => glow = value; }

    [SerializeField]
    SpriteRenderer _renderer;
    public SpriteRenderer Renderer { get => _renderer; private set => _renderer = value; }
    

    bool isShaking = false;

    public void JsonInitialisation(JsonQuadData jsondata)
    {
        QuadShakeStrength = jsondata.QuadShakeStrength;
        TextShakeStrength = jsondata.TextShakeStrength;
        QuadName = jsondata.QuadName;
        TextPosition = jsondata.TextPosition;
        TextScale = jsondata.TextScale;
        InitialPosition = jsondata.InitialPosition;
        Transform.position = initialPosition;
        Renderer.color = jsondata.Color;
        Level = jsondata.Level;
        Transform.localScale = jsondata.Scale;
        Glow = jsondata.Glow;
        Renderer.sortingOrder = Level - 1;
    }

    void OnBecameVisible()
    {
        if (GameManager.instance.MapZoomer.ZoomLevel == level && !GameManager.instance.LockedInputs) 
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
        if (Renderer.isVisible && GameManager.instance.MapZoomer.ZoomLevel == level)
        {
            GetFromPool();
        }
    }

    void GetFromPool()
    {
        if (Text == null) 
        {
            //Get Text from object pooler
            Text = GameManager.instance.Pool.Pull();
            //set the wanted scale and position
            Transform textTransform = Text.transform;
            textTransform.localPosition = TextPosition;
            textTransform.localScale = TextScale;
            Text.text = QuadName;
            Text.gameObject.SetActive(true);
        }
    }

    void GiveToPool()
    {
        if (Text != null) 
        {
            if (isShaking)
            {
                DOTween.Kill(Text.rectTransform);
            }
            //release the text to the object pooler
            GameManager.instance.Pool.Release(Text);
            //disable it
            Text = null;
        }
    }

    public void Initialize(Transform toCopy, string quadName, int level)
    {
        TextPosition = toCopy.localPosition;
        TextScale = toCopy.localScale;
        this.QuadName = quadName;
        InitialPosition = Transform.localPosition;
        Level = level;
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
        textTransform.DOShakeAnchorPos(0.5f, TextShakeStrength * Transform.lossyScale.x).OnComplete(ResetTextPosition);
        Transform.DOShakePosition(0.5f, Transform.lossyScale.x * QuadShakeStrength, 20, 0).OnComplete(ResetQuadPosition);
        GameManager.instance.QuadCounter.ShowResult();
    }

    void ResetTextPosition()
    {
        isShaking = false;
        Text.transform.localPosition = TextPosition;
    }

    void ResetQuadPosition()
    {
        Transform.localPosition = InitialPosition;
    }
}
