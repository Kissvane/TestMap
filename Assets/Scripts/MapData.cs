using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapData : MonoBehaviour
{
    public Camera Camera;
    public List<QuadData> inProcessDatas;
    public QuadData[] datas;
    public QuadData[] Level1;
    public QuadData[] Level2;
    public QuadData[] Level3;
    public QuadData[] Level4;
    public QuadData[][] Levels;
    public SetupQuadBehaviour testQuad;

    public void Initialize()
    {
        Levels = new QuadData[][] { Level1, Level2, Level3, Level4 };
    }

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
        Level1 = inProcessDatas.Where(x => x.Level == 1).ToArray();
        Level2 = inProcessDatas.Where(x => x.Level == 2).ToArray();
        Level3 = inProcessDatas.Where(x => x.Level == 3).ToArray();
        Level4 = inProcessDatas.Where(x => x.Level == 4).ToArray();
        inProcessDatas.Clear();
    }
}
