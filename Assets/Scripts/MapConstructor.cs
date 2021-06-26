using System.Collections.Generic;
using UnityEngine;

public class MapConstructor : MonoBehaviour
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
    public List<HashSet<QuadBehaviour>> Levels { get => levels; private set => levels = value; }
    private List<HashSet<QuadBehaviour>> levels = new List<HashSet<QuadBehaviour>>();
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
        SetupCanvas.gameObject.SetActive(false);
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
        QuadBehaviour behaviour = FirstQuad.GetComponentInChildren<QuadBehaviour>();
        
        GetTextData(behaviour, "Universe",1);
        
        Levels.Add(new HashSet<QuadBehaviour> { behaviour });
        Levels.Add(new HashSet<QuadBehaviour>());
        Levels.Add(new HashSet<QuadBehaviour>());
        Levels.Add(new HashSet<QuadBehaviour>());
        
        ConstructLevel2(FirstQuad.transform.GetChild(0).GetChild(0), 2f,2f);
    }

    public void ConstructLevel2(Transform parent, float lineNumber, float columnNumber)
    {
        for (float x = 0; x < lineNumber; x++)
        {
            for (float y = 0; y < columnNumber; y++)
            {
                SpriteRenderer spriteRenderer;
                Transform pivot;
                int totalIndex;
                ProcessQuad(parent, 2, 0.925f, lineNumber, columnNumber, x, y, out spriteRenderer, out pivot, out totalIndex);
                spriteRenderer.color = Colors[totalIndex];
                ConstructLevel3(pivot, 16f, 8f, spriteRenderer.color);
                spriteRenderer.transform.localScale *= 1.075f;
            }
        }
    }

    public void ConstructLevel3(Transform parent, float columnNumber, float lineNumber, Color parentColor)
    {
        for (float x = 0; x < columnNumber; x++)
        {
            for (float y = 0; y < lineNumber; y++)
            {
                SpriteRenderer spriteRenderer;
                Transform pivot;
                int totalIndex;
                ProcessQuad(parent, 3, 0.9f, lineNumber, columnNumber, x, y, out spriteRenderer, out pivot, out totalIndex);
                spriteRenderer.color = GetRandomShade(parentColor);
                ConstructLevel4(pivot, 3f, 3f, spriteRenderer.color);
            }
        }

    }

    public void ConstructLevel4(Transform parent, float lineNumber, float columnNumber, Color colorParent)
    {
        int highlighted = Random.Range(0, 9);

        for (int x = 0; x < lineNumber; x++)
        {
            for (int y = 0; y < columnNumber; y++)
            {
                SpriteRenderer spriteRenderer;
                Transform pivot;
                int totalIndex;
                ProcessQuad(parent, 4, 0.7f, lineNumber, columnNumber, x, y, out spriteRenderer, out pivot, out totalIndex);
                spriteRenderer.color = colorParent;
                //HIGHLIGHT this one
                if (totalIndex == highlighted)
                {
                    if (GlowMode)
                    {
                        spriteRenderer.material = GlowMaterial;
                    }
                    else 
                    {
                        Color.RGBToHSV(colorParent, out float H, out float S, out float V);
                        spriteRenderer.color = Color.HSVToRGB(H, 0.35f, V);
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
    public void ProcessQuad(Transform parent, int level, float scaleReduction, float lineNumber, float columnNumber, float lineIndex, float columnIndex, out SpriteRenderer renderer, out Transform nextPivot, out int totalIndex)
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
        QuadBehaviour behaviour = constructedQuad.GetComponent<QuadBehaviour>();
        GetTextData(behaviour, GetRandomName(), level);
        Levels[level - 1].Add(behaviour);

        Destroy(current.gameObject);
        Destroy(constructedQuad.GetChild(0).gameObject);

        nextPivot = constructedQuad.GetChild(0);
        renderer = r;
    }

    /// <summary>
    /// Store the necessary data in quad to position correctly the text when get it from object pooler
    /// And get the necessary dependencies
    /// </summary>
    /// <param name="quad"></param>
    public void GetTextData(QuadBehaviour quad, string quadName, int level)
    {
        SetupCanvas.SetParent(quad.transform);
        SetupCanvas.localPosition = Vector3.zero;
        SetupCanvas.localScale = Vector3.one * 0.003f;
        Transform textName = SetupCanvas.GetChild(0);
        textName.SetParent(NameCanvas);
        quad.Initialize(textName, quadName, level);
        textName.SetParent(SetupCanvas);
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
