using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

/// <summary>
/// Store map datas
/// </summary>
public class MapData : MonoBehaviour
{
    #region building variables
    public List<QuadData> inProcessDatas;
    public QuadData[] datas;
    #endregion

    #region runtime variables
    public QuadData[] Level1;
    public QuadData[] Level2;
    public QuadData[] Level3;
    public QuadData[] Level4;
    public Dictionary<int, HashSet<QuadData>> Levels;
    #endregion

    public void Initialize()
    {
        Levels = new Dictionary<int, HashSet<QuadData>>();
        Levels.Add(1, new HashSet<QuadData>(Level1));
        Levels.Add(2, new HashSet<QuadData>(Level2));
        Levels.Add(3, new HashSet<QuadData>(Level3));
        Levels.Add(4, new HashSet<QuadData>(Level4));

        Level1 = null;
        Level2 = null;
        Level3 = null;
        Level4 = null;
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
