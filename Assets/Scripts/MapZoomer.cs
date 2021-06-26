using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapZoomer : MonoBehaviour
{
    [SerializeField]
    Camera _camera;
    public Camera Camera { get => _camera; private set => _camera = value; }

    int _zoomLevel = 1;
    public int ZoomLevel { get => _zoomLevel; private set => _zoomLevel = value; }
    
    [SerializeField]
    List<float> ZoomDistances;

    [SerializeField]
    KeyCode ZoomInKey;

    [SerializeField]
    KeyCode ZoomOutKey;

    

    public void FirstZoom()
    {
        for (int i = 2; i < 5; i++)
        {
            ActivateQuads(i, false);
        }
    }

    public void ZoomIn()
    {
        if (_zoomLevel < 4) 
        {
            HideNames(ZoomLevel);
            _zoomLevel++;
            _camera.orthographicSize = ZoomDistances[_zoomLevel - 1];
            ActivateQuads(_zoomLevel,true);
        }
    }


    public void ZoomOut()
    {
        if (_zoomLevel > 1) 
        {
            ActivateQuads(_zoomLevel, false);
            _zoomLevel--;
            _camera.orthographicSize = ZoomDistances[_zoomLevel - 1];
            ActivateQuads(_zoomLevel, true);
        }
    }

    public void ActivateQuads(int level, bool activate)
    {
        Linker.instance.MapConstructor.LevelsParents[level - 1].gameObject.SetActive(activate);
        foreach (QuadBehaviour quad in Linker.instance.MapConstructor.Levels[level - 1])
        {
            quad.EnableTextIfAlreadyVisible();
        }
    }

    public void HideNames(int level)
    {
        foreach(QuadBehaviour quad in Linker.instance.MapConstructor.Levels[level - 1])
        {
            quad.DisableText();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(ZoomInKey))
        {
            ZoomIn();
        }
        else if (Input.GetKeyDown(ZoomOutKey))
        {
            ZoomOut();
        }
    }
}
