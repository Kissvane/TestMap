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

    /// <summary>
    /// Update the IsActivated state of quadDatas using zoom level
    /// </summary>
    /// <param name="Level">the zoom level to use</param>
    public void ActivateQuads(int Level)
    {
        
    }
}
