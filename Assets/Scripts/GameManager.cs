using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    MapBuilder mapBuilder;
    public MapBuilder MapBuilder { get => mapBuilder; private set => mapBuilder = value; }

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
    MapData mapData;
    public MapData MapData { get => mapData; private set => mapData = value; }

    [SerializeField]
    MoveCamera moveCamera;
    public MoveCamera MoveCamera { get => moveCamera; private set => moveCamera = value; }

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

        TextPool.DisableAllObject();
        MapData.Initialize();
        MapZoomer.FirstZoom();
        MoveCamera.UpdateTarget();
    }
    #endregion

}
