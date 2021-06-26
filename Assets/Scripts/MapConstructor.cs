using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class MapConstructor : MonoBehaviour
{
    [SerializeField]
    GameObject Quad;

    [SerializeField]
    float size = 100f;

    [SerializeField]
    GameObject FirstQuad;

    [SerializeField]
    List<Color> Colors;

    [SerializeField]
    private List<Transform> levelsParents;
    public List<Transform> LevelsParents { get => levelsParents; private set => levelsParents = value; }

    /*[SerializeField]
    private List<Transform> levelCanvas;
    public List<Transform> LevelCanvas { get => levelCanvas; private set => levelCanvas = value; }*/

    [SerializeField]
    Transform NameCanvas;

    [SerializeField]
    Transform SetupCanvas;

    public List<HashSet<QuadBehaviour>> Levels { get => levels; private set => levels = value; }
    private List<HashSet<QuadBehaviour>> levels = new List<HashSet<QuadBehaviour>>();

    //public List<HashSet<QuadBehaviour>> levels { get => levels;}

    [SerializeField]
    MapZoomer MapZoomer;

    Stopwatch Stopwatch = new Stopwatch();

    [SerializeField]
    float ShadeRange = 0.2f;

    [SerializeField]
    float MinColorRange;

    [SerializeField]
    float MaxColorRange;

    [SerializeField]
    Sprite QuadWithBorder;

    [SerializeField]
    TextObjectPooler TextObjectPooler;

    // Start is called before the first frame update
    void Start()
    {
        Stopwatch.Start();
        ConstructLevel1();
        SetupCanvas.gameObject.SetActive(false);
        Stopwatch.Stop();
        UnityEngine.Debug.Log(Stopwatch.ElapsedMilliseconds);
        MapZoomer.FirstZoom();
    }

    //some inevitable code repetition but it's the faster and the more easily adaptable solution in the long run
    #region iterative solution
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

    

    public void ConstructLevel2(Transform parent , float lineNumber, float columnNumber)
    {
        for (float x = 0; x < lineNumber; x++)
        {
            for (float y = 0; y < columnNumber; y++)
            {
                Transform current = Instantiate(Quad, parent).transform;
                current.localScale = Vector3.one / Mathf.Max(columnNumber, lineNumber);
                current.localPosition = new Vector3(x / columnNumber, -y / lineNumber, 0f);
                current.GetChild(0).localScale *= 0.925f;
                int totalIndex = (int)(x + y * columnNumber);
                Color currentColor = Colors[totalIndex];
                Transform constructedQuad = current.GetChild(0);
                SpriteRenderer r = constructedQuad.GetComponent<SpriteRenderer>();
                r.color = currentColor;
                r.sortingOrder = 1;
                constructedQuad.SetParent(LevelsParents[1]);
                constructedQuad.localPosition = new Vector3(constructedQuad.localPosition.x, constructedQuad.localPosition.y, 0f);

                QuadBehaviour behaviour = constructedQuad.GetComponent<QuadBehaviour>();
                GetTextData(behaviour, GetRandomName(),2);
                Levels[1].Add(behaviour);
                ConstructLevel3(constructedQuad.GetChild(0), 16f, 8f, currentColor, totalIndex);
                constructedQuad.localScale *= 1.075f;
                Destroy(current.gameObject);
                Destroy(constructedQuad.GetChild(0).gameObject);
            }
        }
    }

    public void ConstructLevel3(Transform parent, float columnNumber, float lineNumber, Color parentColor, int quadrant)
    {
        for (float x = 0; x < columnNumber; x++)
        {
            for (float y = 0; y < lineNumber; y++)
            {
                Transform current = Instantiate(Quad, parent).transform;
                current.localScale /= Mathf.Max(lineNumber, columnNumber);
                current.localPosition = new Vector3(x / (2*lineNumber), (-2*y) / columnNumber, 0f);
                current.GetChild(0).localScale *= 0.7f;

                int totalIndex = (int)(x + y * lineNumber);

                Transform constructedQuad = current.GetChild(0);

                SpriteRenderer r = constructedQuad.GetComponent<SpriteRenderer>();
                r.sprite = QuadWithBorder;
                r.color = GetRandomShade(parentColor);
                r.sortingOrder = 2;

                constructedQuad.SetParent(LevelsParents[2]);
                constructedQuad.localPosition = new Vector3(constructedQuad.localPosition.x, constructedQuad.localPosition.y, 0f);

                QuadBehaviour behaviour = constructedQuad.GetComponent<QuadBehaviour>();
                GetTextData(behaviour, GetRandomName(),3);
                Levels[2].Add(behaviour);
                int column = x / columnNumber < 0.5f ? 0 : 1;
                int line = y / lineNumber < 0.5f ? 0 : 1;

                ConstructLevel4(constructedQuad.GetChild(0), 3f, 3f, r.color, column, line, quadrant);
                Destroy(current.gameObject);
                Destroy(constructedQuad.GetChild(0).gameObject);
            }
        }

    }

    public void ConstructLevel4(Transform parent, float lineNumber, float columnNumber, Color colorParent, int column, int line, int quadrant)
    {
        int highlighted = Random.Range(0,9);

        for (float x = 0; x < lineNumber; x++)
        {
            for (float y = 0; y < columnNumber; y++)
            {
                Transform current = Instantiate(Quad, parent).transform;
                current.localScale = Vector3.one / Mathf.Max(columnNumber, lineNumber);
                current.localPosition = new Vector3(x / columnNumber, -y / lineNumber, 0f);
                current.GetChild(0).localScale *= 0.7f;

                Transform constructedQuad = current.GetChild(0);
                Levels[3].Add(constructedQuad.GetComponent<QuadBehaviour>());

                SpriteRenderer r = constructedQuad.GetComponent<SpriteRenderer>();
                r.sprite = QuadWithBorder;
                r.color = colorParent;
                r.sortingOrder = 3;

                QuadBehaviour behaviour = constructedQuad.GetComponent<QuadBehaviour>();
                GetTextData(behaviour, GetRandomName(), 4);
                constructedQuad.SetParent(LevelsParents[3]);

                //constructedQuad.GetChild(1).GetChild(0).SetParent(LevelCanvas[3+(quadrant*4)+column*2+line].transform);
                constructedQuad.localPosition = new Vector3(constructedQuad.localPosition.x, constructedQuad.localPosition.y, 0f);

                //behaviour.SetName(GetRandomName());
                //behaviour.SaveInitialState();

                if ((int) (x+y*columnNumber) == highlighted)
                {
                    //HIGHLIGHT this one
                    Color.RGBToHSV(colorParent, out float H, out float S, out float V);
                    r.color = Color.HSVToRGB(H, 0.35f , V);
                }
                Destroy(current.gameObject);
                Destroy(constructedQuad.GetChild(0).gameObject);
            }
        }
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

    string GetRandomName()
    {
        int length = Random.Range(1,4);
        string result = "";

        for (int i = 0; i < length; i++)
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
    #endregion

}
