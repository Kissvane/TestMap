using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{

    #region Variables

    [SerializeField]
    GameObject SetupQuadPfb;
    [SerializeField]
    GameObject QuadPfb;
    [SerializeField]
    float size = 100f;
    [SerializeField]
    GameObject FirstQuad;
    [SerializeField]
    List<Color> Colors;
    [SerializeField]
    private List<Transform> levelsParents;
    public List<Transform> LevelsParents { get => levelsParents; private set => levelsParents = value; }
    [SerializeField]
    Transform NameCanvas;
    [SerializeField]
    Transform SetupCanvas;
    [SerializeField]
    float ShadeRange = 0.2f;
    [SerializeField]
    int NameLength = 6;
    [SerializeField]
    Material GlowMaterial;
    [SerializeField]
    bool GlowMode = false;

    Transform temp;

    #endregion

    #region constructions functions
    [ContextMenu("BUILD MAP")]
    public void BuildMap()
    {
        //initialization phase
        temp = new GameObject("TEMP").transform;
        //destroy previously created objects
        foreach (Transform t in LevelsParents)
        {
            if(t != null) DestroyImmediate(t.gameObject);
        }
        LevelsParents.Clear();
        GameManager.instance.MapData.ClearData();
        FirstQuad.transform.SetParent(null);

        for (int i = 0; i < 4; i++)
        {
            Transform t = new GameObject("Level" + (i + 1)).transform;
            t.position = Vector3.zero;
            t.localScale = Vector3.one;
            LevelsParents.Add(t);
        }

        //Start building
        ConstructLevel1();

        //unparent useful object
        FirstQuad.transform.SetParent(null);
        SetupCanvas.SetParent(null);

        //destroy test objects
        foreach (Transform t in LevelsParents)
        {
            DestroyImmediate(t.gameObject);
        }
        LevelsParents.Clear();
        DestroyImmediate(temp.gameObject);

        //hide these construction useful object because they are useless in runtime
        FirstQuad.SetActive(false);
        SetupCanvas.gameObject.SetActive(false);

        for (int i = 0; i < 4; i++)
        {
            Transform t = new GameObject("Level" + (i + 1)).transform;
            t.position = Vector3.zero;
            t.localScale = Vector3.one;
            LevelsParents.Add(t);
        }

        //data process
        GameManager.instance.MapData.TransformDatas();

        foreach (QuadData data in GameManager.instance.MapData.datas)
        {
            GameObject go = Instantiate(QuadPfb, LevelsParents[data.Level-1]);
            data.SetQuad(go.GetComponent<QuadBehaviour>());
        }

        //disabled the built objects
        foreach (Transform t in LevelsParents)
        {
            t.gameObject.SetActive(false);
        }

        GameManager.instance.MapData.ClearData();
        Debug.Log("MAP CONSTRUCTED");
    }

    public void ConstructLevel1()
    {
        FirstQuad.transform.localScale = Vector3.one * size;
        NameCanvas.transform.localScale = Vector3.one * 0.001f * size;

        FirstQuad.transform.SetParent(LevelsParents[0]);
        FirstQuad.transform.localPosition = new Vector3(FirstQuad.transform.localPosition.x, FirstQuad.transform.localPosition.y, 0f);
        SetupQuadBehaviour behaviour = FirstQuad.GetComponentInChildren<SetupQuadBehaviour>();
        QuadData result = GetData(behaviour, "Universe", 1, -1);
        result.Color = Color.white;
        
        ConstructLevel2(FirstQuad.transform.GetChild(0).GetChild(0), 0,2f,2f);
    }

    public void ConstructLevel2(Transform parent, int fatherIndex,float lineNumber, float columnNumber)
    {
        for (float x = 0; x < lineNumber; x++)
        {
            for (float y = 0; y < columnNumber; y++)
            {
                SpriteRenderer spriteRenderer;
                Transform pivot;
                int totalIndex;

                QuadData result = ProcessQuad(parent, fatherIndex,2, 0.925f, lineNumber, columnNumber, x, y, out spriteRenderer, out pivot, out totalIndex);
                spriteRenderer.color = Colors[totalIndex];
                
                ConstructLevel3(pivot, GameManager.instance.MapData.inProcessDatas.Count - 1, 16f, 8f, spriteRenderer.color);
                result.Color = spriteRenderer.color;
                spriteRenderer.transform.localScale *= 1.075f;
                result.Scale = spriteRenderer.transform.localScale;
            }
        }
    }

    public void ConstructLevel3(Transform parent, int fatherIndex,float columnNumber, float lineNumber, Color parentColor)
    {
        for (float x = 0; x < columnNumber; x++)
        {
            for (float y = 0; y < lineNumber; y++)
            {
                SpriteRenderer spriteRenderer;
                Transform pivot;
                int totalIndex;
                QuadData result = ProcessQuad(parent, fatherIndex,3, 0.9f, lineNumber, columnNumber, x, y, out spriteRenderer, out pivot, out totalIndex);
                spriteRenderer.color = GetRandomShade(parentColor);
                result.Color = spriteRenderer.color;
                ConstructLevel4(pivot, GameManager.instance.MapData.inProcessDatas.Count - 1, 3f, 3f, spriteRenderer.color);
            }
        }

    }

    public void ConstructLevel4(Transform parent, int fatherIndex,float lineNumber, float columnNumber, Color colorParent)
    {
        int highlighted = Random.Range(0, 9);

        for (int x = 0; x < lineNumber; x++)
        {
            for (int y = 0; y < columnNumber; y++)
            {
                SpriteRenderer spriteRenderer;
                Transform pivot;
                int totalIndex;
                QuadData result = ProcessQuad(parent, fatherIndex,4, 0.7f, lineNumber, columnNumber, x, y, out spriteRenderer, out pivot, out totalIndex);
                spriteRenderer.color = colorParent;
                result.Color = spriteRenderer.color;
                //HIGHLIGHT this one
                if (totalIndex == highlighted)
                {
                    if (GlowMode)
                    {
                        spriteRenderer.material = GlowMaterial;
                        result.Glow = true;
                    }
                    else 
                    {
                        Color.RGBToHSV(colorParent, out float H, out float S, out float V);
                        spriteRenderer.color = Color.HSVToRGB(H, 0.35f, V);
                        result.Color = spriteRenderer.color;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Manage scale, position and initialisation of the newly created quads
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="level"></param>
    /// <param name="scaleReduction"></param>
    /// <param name="lineNumber"></param>
    /// <param name="columnNumber"></param>
    /// <param name="lineIndex"></param>
    /// <param name="columnIndex"></param>
    /// <param name="renderer"></param>
    /// <param name="nextPivot"></param>
    /// <param name="totalIndex"></param>
    public QuadData ProcessQuad(Transform parent, int fatherIndex,int level, float scaleReduction, float lineNumber, float columnNumber, float lineIndex, float columnIndex, out SpriteRenderer renderer, out Transform nextPivot, out int totalIndex)
    {
        Transform current = Instantiate(SetupQuadPfb, parent).transform;
        //set the good scale and position to the instantiated SetupQuadBehaviour parent
        current.localScale = Vector3.one * (1f / Mathf.Max(lineNumber, columnNumber));
        current.localPosition = new Vector3(lineIndex / columnNumber, -columnIndex / lineNumber, 0f);
        //reduce scale to avoid overlapping with parent limit
        current.GetChild(0).localScale *= scaleReduction;

        totalIndex = (int)(lineIndex + columnIndex * lineNumber);
        Transform constructedQuad = current.GetChild(0);
        SpriteRenderer r = constructedQuad.GetComponent<SpriteRenderer>();

        r.sortingOrder = level - 1;
        constructedQuad.SetParent(LevelsParents[level - 1]);
        constructedQuad.localPosition = new Vector3(constructedQuad.localPosition.x, constructedQuad.localPosition.y, 0f);
        SetupQuadBehaviour behaviour = constructedQuad.GetComponent<SetupQuadBehaviour>();
        //create the quadData from SetupQuadBehaviour datas
        QuadData result = GetData(behaviour, level == 3 ? totalIndex.ToString() : GetRandomName(), level, fatherIndex);
        //destroy the useless parent
        DestroyImmediate(current.gameObject);

        nextPivot = constructedQuad.GetChild(0);
        renderer = r;

        return result;
    }

    /// <summary>
    /// Store the necessary data in quad to position correctly the text when get it from object pooler
    /// And get the necessary dependencies
    /// </summary>
    /// <param name="quad"></param>
    public QuadData GetData(SetupQuadBehaviour quad, string quadName, int level, int fatherIndex)
    {
        //parent temp to quad and imitate pos and scale
        temp.SetParent(quad.transform);
        temp.localPosition = Vector3.zero;
        temp.localScale = Vector3.one;

        //parent SetupCanvas to quad and set the right values
        //theses values were found experimenting in editor
        SetupCanvas.SetParent(quad.Transform);
        SetupCanvas.localPosition = Vector3.zero;
        SetupCanvas.localScale = Vector3.one * 0.003f;

        //Set the test text in NameCanvas
        Transform textName = SetupCanvas.GetChild(0);
        textName.localPosition = Vector3.zero;
        textName.localScale = Vector3.one;
        textName.SetParent(NameCanvas);
        //save useful datas in quad
        quad.Initialize(textName, quadName, level);
        //reparent the test text to SetupCanvas
        textName.SetParent(SetupCanvas);
        QuadData result = new QuadData(quad, level != 1);
        //its impossible to change transform parent of an object inside a prefab when in edit mode
        temp.SetParent(null);
        //so I use this temporary transform to store the right scale
        result.Scale = temp.localScale;
        //store the quadData in a temp list
        GameManager.instance.MapData.inProcessDatas.Add(result);
        return result;
    }

    #endregion

    string GetRandomName()
    {
        string result = "";

        for (int i = 0; i < NameLength; i++)
        {
            result = string.Concat(result, (char) Random.Range(65, 90));
        }

        return result;
    }

    Color GetRandomShade(Color original)
    {
        Color.RGBToHSV(original, out float H, out float S, out float V);
        return Color.HSVToRGB(H, Random.Range(0.5f + ShadeRange, 0.5f + ShadeRange * 2), V + Random.Range(-ShadeRange / 2f, ShadeRange / 2f));
    }
    

}
