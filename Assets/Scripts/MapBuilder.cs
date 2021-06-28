using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{

    #region Variables

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
    public List<HashSet<AbstractQuad>> Levels { get => levels; private set => levels = value; }
    private List<HashSet<AbstractQuad>> levels = new List<HashSet<AbstractQuad>>();
    [SerializeField]
    float ShadeRange = 0.2f;
    [SerializeField]
    int NameLength = 6;
    [SerializeField]
    Material GlowMaterial;
    [SerializeField]
    bool GlowMode = false;

    #endregion

    #region monobehaviours callbacks
    // Start is called before the first frame update
    void Start()
    {
        ConstructLevel1();
        FirstQuad.transform.GetChild(1).SetParent(null);
        FirstQuad.transform.GetChild(1).SetParent(null);
        foreach (Transform t in levelsParents)
        {
            Destroy(t.gameObject);
        }
        Destroy(FirstQuad);
        SetupCanvas.gameObject.SetActive(false);
        Linker.instance.QuadManager.TransformDatas();
        Linker.instance.MapZoomer.FirstZoom();
    }

    #endregion

    #region constructions functions
    public void ConstructLevel1()
    {
        FirstQuad.transform.localScale = Vector3.one * size;
        NameCanvas.transform.localScale *= size;

        FirstQuad.transform.SetParent(LevelsParents[0]);
        FirstQuad.transform.localPosition = new Vector3(FirstQuad.transform.localPosition.x, FirstQuad.transform.localPosition.y, 0f);
        SetupQuadBehaviour behaviour = FirstQuad.GetComponentInChildren<SetupQuadBehaviour>();

        QuadData result = GetData(behaviour, "Universe", 1, -1);
        result.Color = Color.white;
        
        Levels.Add(new HashSet<AbstractQuad> { behaviour });
        Levels.Add(new HashSet<AbstractQuad>());
        Levels.Add(new HashSet<AbstractQuad>());
        Levels.Add(new HashSet<AbstractQuad>());
        
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
                
                ConstructLevel3(pivot, Linker.instance.QuadManager.inProcessDatas.Count - 1, 16f, 8f, spriteRenderer.color);
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
                ConstructLevel4(pivot, Linker.instance.QuadManager.inProcessDatas.Count - 1, 3f, 3f, spriteRenderer.color);
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
                        result.SetGlow();
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
        Transform current = Instantiate(QuadPfb, parent).transform;
        current.localScale /= Mathf.Max(lineNumber, columnNumber);
        current.localPosition = new Vector3(lineIndex / columnNumber, -columnIndex / lineNumber, 0f);
        current.GetChild(0).localScale *= scaleReduction;
        totalIndex = (int)(lineIndex + columnIndex * lineNumber);
        Transform constructedQuad = current.GetChild(0);
        SpriteRenderer r = constructedQuad.GetComponent<SpriteRenderer>();

        r.sortingOrder = level - 1;
        constructedQuad.SetParent(LevelsParents[level - 1]);
        constructedQuad.localPosition = new Vector3(constructedQuad.localPosition.x, constructedQuad.localPosition.y, 0f);
        SetupQuadBehaviour behaviour = constructedQuad.GetComponent<SetupQuadBehaviour>();
        QuadData result = GetData(behaviour, level == 3 ? totalIndex.ToString() : GetRandomName(), level, fatherIndex);
        Levels[level - 1].Add(behaviour);

        Destroy(current.gameObject);
        Destroy(constructedQuad.GetChild(0).gameObject);

        nextPivot = constructedQuad.GetChild(0);
        renderer = r;

        Linker.instance.QuadManager.AddToTree(fatherIndex, Linker.instance.QuadManager.inProcessDatas.Count-1);

        return result;
    }

    /// <summary>
    /// Store the necessary data in quad to position correctly the text when get it from object pooler
    /// And get the necessary dependencies
    /// </summary>
    /// <param name="quad"></param>
    public QuadData GetData(SetupQuadBehaviour quad, string quadName, int level, int fatherIndex)
    {
        SetupCanvas.SetParent(quad.Transform);
        SetupCanvas.localPosition = Vector3.zero;
        SetupCanvas.localScale = Vector3.one * 0.003f;
        Transform textName = SetupCanvas.GetChild(0);
        textName.SetParent(NameCanvas);
        quad.Initialize(textName, quadName, level);
        textName.SetParent(SetupCanvas);
        Transform oldParent = quad.transform.parent;
        int oldSiblingIndex = quad.transform.GetSiblingIndex();
        quad.transform.SetParent(null);
        QuadData result = new QuadData(quad, level != 1);
        Linker.instance.QuadManager.inProcessDatas.Add(result);
        result.QuadFatherIndex = fatherIndex;
        quad.transform.SetParent(oldParent);
        quad.transform.SetSiblingIndex(oldSiblingIndex);
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
