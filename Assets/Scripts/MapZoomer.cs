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
        UpdateQuadStates();
    }

    public void ZoomIn()
    {
        if (ZoomLevel < 4) 
        {
            ZoomLevel++;
            Linker.instance.MoveCamera.StopCamera();
            Camera.orthographicSize = ZoomDistances[ZoomLevel - 1];
            UpdateQuadStates();
        }
    }


    public void ZoomOut()
    {
        if (_zoomLevel > 1) 
        {
            ZoomLevel--;
            Linker.instance.MoveCamera.StopCamera();
            Camera.orthographicSize = ZoomDistances[ZoomLevel - 1];
            UpdateQuadStates();
        }
    }

    void UpdateQuadStates()
    {
        Linker.instance.MapData.ActivateQuads(ZoomLevel);
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
