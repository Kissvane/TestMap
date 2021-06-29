using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage zoom level
/// </summary>
public class MapZoomer : MonoBehaviour
{
    #region variables
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
    #endregion

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
        foreach (QuadData data in GameManager.instance.MapData.Levels[level])
        {
            data.Quad.EnableTextIfAlreadyVisible();
        }
    }

    public void HideNames(int level)
    {
        foreach (QuadData data in GameManager.instance.MapData.Levels[level])
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