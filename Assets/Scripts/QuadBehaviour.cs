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

/// <summary>
/// Manage data storing and name showing
/// </summary>
public class QuadBehaviour : MonoBehaviour
{
    #region variables
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

    #endregion

    #region Init functions
    /// <summary>
    /// QuadBehaviour rehydratation function
    /// </summary>
    /// <param name="jsondata">the data used</param>
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

    public void Initialize(Transform toCopy, string quadName, int level)
    {
        TextPosition = toCopy.localPosition;
        TextScale = toCopy.localScale;
        this.QuadName = quadName;
        InitialPosition = Transform.localPosition;
        Level = level;
    }
    #endregion

    #region visibility behaviours
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
    #endregion

    #region name behaviours
    public void DisableText()
    {
        GiveToPool();
    }

    /// <summary>
    /// Fade the text then release it to the pool
    /// </summary>
    /// <param name="fadeTime"> fade duration </param>
    public void FadeText(float fadeTime)
    {
        if (Renderer.isVisible) 
        {
            if (GameManager.instance.HideFade) 
            {
                Text.DOFade(0f, fadeTime).OnComplete(GiveToPool);
            }
            else
            {
                GiveToPool();
            }
        }
    }

    public void EnableTextIfAlreadyVisible(bool fade = false, float fadeTime = 0f)
    {
        if (Renderer.isVisible && GameManager.instance.MapZoomer.ZoomLevel == level)
        {
            GetFromPool(fade && GameManager.instance.ApparationFade, fadeTime);
        }
    }

    /// <summary>
    /// Pick a text from pool. And alter text parameters to make it quadName
    /// </summary>
    /// <param name="fade"> use fade to show the text </param>
    /// <param name="fadeTime"> show fade duration </param>
    void GetFromPool(bool fade = false, float fadeTime = 0f)
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
            //fade management
            if (fade) 
            {
                Color color = Text.color;
                color.a = 0f;
                Text.color = color;
                Text.DOFade(1f, fadeTime);
            }
            //be sure the wanted text is not transparent
            else
            {
                Color color = Text.color;
                color.a = 1f;
                Text.color = color;
            }
        }
    }

    /// <summary>
    /// Release this quadBehaviour name in the pool
    /// </summary>
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

    void ResetTextPosition()
    {
        isShaking = false;
        Text.transform.localPosition = TextPosition;
    }

    void ResetQuadPosition()
    {
        Transform.localPosition = InitialPosition;
    }

    #endregion

    /// <summary>
    /// manage click feedback
    /// </summary>
    public void ClickFeedback()
    {
        //kill ongoing shake
        DOTween.Kill(Transform);
        RectTransform textTransform = Text.rectTransform;
        DOTween.Kill(textTransform.transform);
        //reset quad and name position to prevent quad displacement when a shake is interrupting another
        ResetQuadPosition();
        ResetTextPosition();
        isShaking = true;
        //shake the name
        textTransform.DOShakeAnchorPos(0.5f, TextShakeStrength * Transform.lossyScale.x).OnComplete(ResetTextPosition);
        //shake the quad
        Transform.DOShakePosition(0.5f, Transform.lossyScale.x * QuadShakeStrength, 20, 0).OnComplete(ResetQuadPosition);
    }

    
}
