using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Datas allowing to build or a reconstruct a quad
/// </summary>
[System.Serializable]
public class QuadData
{
    #region variables
    [SerializeField]
    Vector3 position;
    public Vector3 Position { get => position; private set => position = value; }
    
    [SerializeField]
    private Vector3 scale;
    public Vector3 Scale { get => scale; set => scale = value; }
    
    [SerializeField]
    private string name;
    public string Name { get => name; private set => name = value; }

    [SerializeField]
    private int level;
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    private bool borders;
    public bool Borders { get => borders; private set => borders = value; }
    
    [SerializeField]
    private float quadShakeStrength = 1f;
    public float QuadShakeStrength { get => quadShakeStrength; private set => quadShakeStrength = value; }
    
    [SerializeField]
    private float textShakeStrength = 1f;
    public float TextShakeStrength { get => textShakeStrength; private set => textShakeStrength = value; }
    
    [SerializeField]
    private Vector3 textPosition;
    public Vector3 TextPosition { get => textPosition; private set => textPosition = value; }
    
    [SerializeField]
    private Vector3 textScale;
    public Vector3 TextScale { get => textScale; private set => textScale = value; }
    
    [SerializeField]
    private Color color;
    public Color Color { get => color; set => color = value; }
    
    [SerializeField]
    private QuadBehaviour quad;
    public QuadBehaviour Quad { get => quad; private set => quad = value; }

    [SerializeField]
    private bool glow = false;
    public bool Glow { get => glow; set => glow = value; }
    #endregion

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="quad">datas used to construct this one</param>
    /// <param name="borders">does this quad have borders</param>
    public QuadData(SetupQuadBehaviour quad, bool borders)
    {
        Position = quad.Transform.position;
        Scale = quad.Transform.localScale;
        TextPosition = quad.TextPosition;
        TextScale = quad.TextScale;
        Name = quad.QuadName;
        Level = quad.Level;
        this.Borders = borders;
        Color = quad.Renderer.color;
        QuadShakeStrength = quad.QuadShakeStrength;
        TextShakeStrength = quad.TextShakeStrength;
    }

    /// <summary>
    /// Use a quad behaviour to visualize this quadData
    /// </summary>
    /// <param name="quad">the quad behaviour used</param>
    public void SetQuad(QuadBehaviour quad)
    {
        Quad = quad;
        Quad.UseDatas(this);
    }
}
