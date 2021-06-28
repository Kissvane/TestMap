using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapData : MonoBehaviour
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

    /// <summary>
    /// store the final datas in an array because it's more performant to use
    /// </summary>
    public void TransformDatas()
    {
        datas = inProcessDatas.ToArray();
        inProcessDatas.Clear();
    }

    public void AddToTree(int fatherIndex, int childIndex)
    {
        inProcessDatas[fatherIndex].AddChild(childIndex);
    }

    /// <summary>
    /// Test the visibility of all quadDatas
    /// </summary>
    public void OptimizedTestVisibility()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera);
        DeepTestVisibility(planes, datas[0]);
    }

    /// <summary>
    /// Test only necessary quads
    /// </summary>
    /// <param name="planes">camera frustrum</param>
    /// <param name="data"> data representing one quad of the map </param>
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

    /// <summary>
    /// Update the IsActivated state of quadDatas using zoom level
    /// </summary>
    /// <param name="Level">the zoom level to use</param>
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
