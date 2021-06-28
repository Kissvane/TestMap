using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuadData
{
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
    bool wasVisibleLastFrame = false;
    public bool WasVisibleLastFrame { get => wasVisibleLastFrame; private set => wasVisibleLastFrame = value; }

    [SerializeField]
    private bool isVisibleThisFrame = false;
    public bool IsVisibleThisFrame { get => isVisibleThisFrame; private set => isVisibleThisFrame = value; }
    
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
    public Color Color { get => color; private set => color = value; }
    
    [SerializeField]
    private QuadBehaviour quad;
    public QuadBehaviour Quad { get => quad; private set => quad = value; }
    
    [SerializeField]
    private bool isActivated = false;
    public bool IsActivated { get => isActivated; set => isActivated = value; }
    
    [SerializeField]
    private bool glow = false;
    public bool Glow { get => glow; set => glow = value; }
    
    [SerializeField]
    private int quadFatherIndex = -1;
    public int QuadFatherIndex { get => quadFatherIndex; set => quadFatherIndex = value; }
    
    [SerializeField]
    private List<int> childrenIndexes = new List<int>();
    public List<int> ChildrenIndexes { get => childrenIndexes; private set => childrenIndexes = value; }




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

    public void AddChild(int data)
    {
        ChildrenIndexes.Add(data);
    }

    public void SetColor(Color color)
    {
        Color = color;
    }

    public void TestVisibility(SetupQuadBehaviour testQuad, Plane[] planes)
    {
        testQuad.Transform.position = Position;
        testQuad.Transform.localScale = Scale;
        IsVisibleThisFrame = GeometryUtility.TestPlanesAABB(planes, testQuad.Renderer.bounds);

        if (IsActivated && IsVisibleThisFrame)
        {
            if (Quad == null)
            {
                SetQuad(Linker.instance.QuadPool.Pull());
            }
            else
            {
                if (Level != Linker.instance.MapZoomer.ZoomLevel && Quad.Text != null)
                {
                    Quad.GiveToPool();
                }
                else if(Level == Linker.instance.MapZoomer.ZoomLevel && Quad.Text == null)
                {
                    Quad.GetFromPool();
                }
            }
        }
        else if((!IsActivated || !IsVisibleThisFrame) && Quad != null)
        {
            UnsetQuad();
        }
        WasVisibleLastFrame = IsVisibleThisFrame;
    }

    public void SetQuad(QuadBehaviour quad)
    {
        Quad = quad;
        Quad.UseDatas(this);
    }

    public void UnsetQuad()
    {
        Quad.ClearData();
        Linker.instance.QuadPool.Release(Quad);
        Quad = null;
    }
}
