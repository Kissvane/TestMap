using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuadData
{
    public Vector3 Position;
    public Vector3 Scale;
    public string Name;
    public int Level;
    public bool Borders;
    public bool WasVisibleLastFrame = false;
    public bool IsVisibleThisFrame = false;
    public float QuadShakeStrength = 1f;
    public float TextShakeStrength = 1f;
    public Vector3 textPosition;
    public Vector3 textScale;
    public Color Color;
    public QuadBehaviour Quad;
    public bool IsActivated = false;
    public bool Glow = false;
    public int QuadFatherIndex = -1;
    public List<int> ChildrenIndexes = new List<int>();

    public QuadData(SetupQuadBehaviour quad, bool borders)
    {
        Position = quad.Transform.position;
        Scale = quad.Transform.localScale;
        textPosition = quad.TextPosition;
        textScale = quad.TextScale;
        Name = quad.QuadName;
        Level = quad.Level;
        Borders = borders;
        Color = quad.Renderer.color;
        QuadShakeStrength = quad.QuadShakeStrength;
        TextShakeStrength = quad.TextShakeStrength;
    }

    public void AddChild(int data)
    {
        ChildrenIndexes.Add(data);
    }

    public void Activate(bool value)
    {
        IsActivated = value;
    }

    public void SetGlow()
    {
        Glow = true;
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
