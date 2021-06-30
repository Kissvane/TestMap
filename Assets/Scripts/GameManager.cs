using UnityEngine;

/// <summary>
/// Manage dependencies between all communicating scripts 
/// </summary>
public class GameManager : MonoBehaviour
{
    #region variables
    [SerializeField]
    MapBuilder mapConstructor;
    public MapBuilder MapConstructor { get => mapConstructor; private set => mapConstructor = value; }

    [SerializeField]
    MapZoomer mapZoomer;
    public MapZoomer MapZoomer { get => mapZoomer; private set => mapZoomer = value; }

    [SerializeField]
    TextObjectPooler pool;
    public TextObjectPooler Pool { get => pool; set => pool = value; }

    [SerializeField]
    QuadCounter quadCounter;
    public QuadCounter QuadCounter { get => quadCounter; private set => quadCounter = value; }

    [SerializeField]
    bool lockedInputs = false;
    public bool LockedInputs { get => lockedInputs; private set => lockedInputs = value; }

    [SerializeField]
    bool apparitionFade = false;
    public bool ApparationFade { get => apparitionFade; set => apparitionFade = value; }

    [SerializeField]
    bool hideFade = false;
    public bool HideFade { get => hideFade; set => hideFade = value; }

    public GameObject UiCanvas;
    #endregion

    #region singleton
    static GameManager _instance;
    public static GameManager instance { get { return _instance; } }


    GameManager()
    {
        _instance = this;
    }

    private void Awake()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Linker");
        if (go != gameObject)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public void LockInputs(bool value)
    {
        LockedInputs = value;
        UiCanvas.SetActive(!value);
    }
}
