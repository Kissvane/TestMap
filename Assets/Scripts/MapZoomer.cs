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
        ActivateQuads(1, true);
    }

    public void ZoomIn()
    {
        if (_zoomLevel < 4)
        {
            HideNames(ZoomLevel);
            _zoomLevel++;
            _camera.orthographicSize = ZoomDistances[_zoomLevel - 1];
            ActivateQuads(_zoomLevel, true);
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
        GameManager.instance.MapBuilder.LevelsParents[level - 1].gameObject.SetActive(activate);
        foreach (QuadData data in GameManager.instance.MapData.Levels[level - 1])
        {
            data.Quad.EnableTextIfAlreadyVisible();
        }
    }

    public void HideNames(int level)
    {
        foreach (QuadData data in GameManager.instance.MapData.Levels[level - 1])
        {
            data.Quad.DisableText();
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