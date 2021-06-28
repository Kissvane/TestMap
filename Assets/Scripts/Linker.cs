using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linker : MonoBehaviour
{
    [SerializeField]
    MapBuilder mapConstructor;
    public MapBuilder MapConstructor { get => mapConstructor; private set => mapConstructor = value; }

    [SerializeField]
    MapZoomer mapZoomer;
    public MapZoomer MapZoomer { get => mapZoomer; private set => mapZoomer = value; }

    [SerializeField]
    TextObjectPooler textPool;
    public TextObjectPooler TextPool { get => textPool; private set => textPool = value; }

    [SerializeField]
    QuadObjectPooler quadPool;
    public QuadObjectPooler QuadPool { get => quadPool; private set => quadPool = value; }

    [SerializeField]
    QuadCounter quadCounter;
    public QuadCounter QuadCounter { get => quadCounter; private set => quadCounter = value; }

    [SerializeField]
    MapData quadManager;
    public MapData QuadManager { get => quadManager; private set => quadManager = value; }

    [SerializeField]
    MoveCamera moveCamera;
    public MoveCamera MoveCamera { get => moveCamera; private set => moveCamera = value; }

    #region singleton
    static Linker _instance;
    public static Linker instance { get { return _instance; } }

    Linker()
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
}
