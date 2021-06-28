using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuadManager : MonoBehaviour
{
    public Camera Camera;
    public List<QuadData> inProcessDatas;
    public QuadData[] datas;
    public SetupQuadBehaviour testQuad;

    public void ClearData()
    {
        inProcessDatas.Clear();
        datas = null;
    }

    public void TransformDatas()
    {
        datas = inProcessDatas.ToArray();
        inProcessDatas.Clear();
    }

    public void AddToTree(int fatherIndex, int childIndex)
    {
        inProcessDatas[fatherIndex].AddChild(childIndex);
    }

    public void TestVisibility()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera);

        foreach (QuadData data in datas)
        {
            data.TestVisibility(testQuad, planes);
        }
    }

    public void OptimizedTestVisibility()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera);
        DeepTestVisibility(planes, datas[0]);
    }

    public void DeepTestVisibility(Plane[] planes, QuadData data)
    {
        data.TestVisibility(testQuad, planes);
        if (!data.IsActivated || (!data.IsVisibleThisFrame && !data.WasVisibleLastFrame))
        {
            return;
        }
        else
        {
            for (int i = 0; i < data.ChildrenIndexes.Count; i++)
            {
                DeepTestVisibility(planes, datas[data.ChildrenIndexes[i]]);
            }
        }
    }

    public void ActivateQuads(int Level)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera);

        foreach (QuadData data in datas)
        {
            data.IsActivated = data.Level <= Level;
        }

        OptimizedTestVisibility();
    }
}
