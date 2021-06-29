using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    [SerializeField]
    Ease Ease;

    [SerializeField]
    float zoomTime = 0.5f;


    public void FirstZoom()
    {
        ActivateQuads(1, true);
    }

    public void ZoomIn()
    {
        if (_zoomLevel < 4)
        {
            GameManager.instance.LockInputs(true);
            HideNames(ZoomLevel);
            _zoomLevel++;
            _camera.DOOrthoSize(ZoomDistances[ZoomLevel - 1], zoomTime).SetEase(Ease).OnComplete(UnlockInputs);
            StartCoroutine(ActivateQuadsAsync(ZoomLevel, true, zoomTime+0.1f));
        }
    }

    public void ZoomOut()
    {
        if (ZoomLevel > 1)
        {
            GameManager.instance.LockInputs(true);
            ActivateQuads(ZoomLevel, false);
            ZoomLevel--;
            _camera.DOOrthoSize(ZoomDistances[ZoomLevel - 1], zoomTime).SetEase(Ease).OnComplete(UnlockInputs);
            ActivateQuads(ZoomLevel, true);
            //StartCoroutine(ActivateQuadsAsync(ZoomLevel, true, zoomTime+0.1f));
        }
    }

    void UnlockInputs()
    {
        GameManager.instance.LockInputs(false);
        //ActivateQuads(ZoomLevel, true);
    }

    public void ActivateQuads(int level, bool activate)
    {
        GameManager.instance.MapConstructor.LevelsParents[level - 1].gameObject.SetActive(activate);
        foreach (QuadBehaviour quad in GameManager.instance.MapConstructor.Levels[level - 1])
        {
            quad.EnableTextIfAlreadyVisible();
        }
    }

    IEnumerator ActivateQuadsAsync(int level, bool activate, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ActivateQuads(level, activate);
    }

    public void HideNames(int level)
    {
        foreach (QuadBehaviour quad in GameManager.instance.MapConstructor.Levels[level - 1])
        {
            quad.DisableText();
        }
    }

#if UNITY_EDITOR || UNITY_STANDALONE
    private void Update()
    {
        if (!GameManager.instance.LockedInputs) 
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
#endif
}
