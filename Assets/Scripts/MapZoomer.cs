using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;


/// <summary>
/// Manage zoom
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

    [SerializeField]
    Ease Ease;

    [SerializeField]
    float zoomTime = 0.5f;
    #endregion

    public void FirstZoom()
    {
        ActivateQuads(1, true);
    }

    public void ZoomIn()
    {
        if (_zoomLevel < 4)
        {
            GameManager.instance.LockInputs(true);
            FadeNames(ZoomLevel, zoomTime * 0.4f);
            _zoomLevel++;
            _camera.DOOrthoSize(ZoomDistances[ZoomLevel - 1], zoomTime).SetEase(Ease).OnComplete(UnlockInputs);
            StartCoroutine(ActivateQuadsAsync(ZoomLevel, true, zoomTime * 0.6f));
            StartCoroutine(AsyncActivateNames(ZoomLevel, zoomTime));
        }
    }

    public void ZoomOut()
    {
        if (ZoomLevel > 1)
        {
            GameManager.instance.LockInputs(true);
            StartCoroutine(ActivateQuadsAsync(ZoomLevel, false, zoomTime * 0.4f));
            ZoomLevel--;
            _camera.DOOrthoSize(ZoomDistances[ZoomLevel - 1], zoomTime).SetEase(Ease).OnComplete(UnlockInputs);
            StartCoroutine(AsyncActivateNames(ZoomLevel, zoomTime));
        }
    }

    /// <summary>
    /// unlock inputs and ui buttons
    /// </summary>
    void UnlockInputs()
    {
        GameManager.instance.LockInputs(false);
    }

    /// <summary>
    /// Activate quads fom a specific level
    /// </summary>
    /// <param name="level">the specific level to alter</param>
    /// <param name="activate">define if it's an activation or disactivation</param>
    public void ActivateQuads(int level, bool activate)
    {
        GameManager.instance.MapConstructor.LevelsParents[level - 1].gameObject.SetActive(activate);
        //name management
        if (activate)
        {
            foreach (QuadBehaviour quad in GameManager.instance.MapConstructor.Levels[level - 1])
            {
                quad.EnableTextIfAlreadyVisible();
            }
        }
        else
        {
            foreach (QuadBehaviour quad in GameManager.instance.MapConstructor.Levels[level - 1])
            {
                quad.DisableText();
            }
        }
    }

    /// <summary>
    /// Asynchronous activation/disactivation of quads from a specific level
    /// </summary>
    /// <param name="level"> the selected level </param>
    /// <param name="activate"> define if it's an activation or disactivation </param>
    /// <param name="waitTime"> wait time before performing the operation </param>
    /// <returns></returns>
    IEnumerator ActivateQuadsAsync(int level, bool activate, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ActivateQuads(level, activate);
    }

    /// <summary>
    /// Asynchronous name management on a specific level 
    /// </summary>
    /// <param name="level"> the selected level </param>
    /// <param name="activate"> define if it's an activation or disactivation </param>
    /// <param name="waitTime"> wait time before performing the operation </param>
    /// <returns></returns>
    IEnumerator AsyncActivateNames(int level, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        foreach (QuadBehaviour quad in GameManager.instance.MapConstructor.Levels[level - 1])
        {
            quad.EnableTextIfAlreadyVisible(true, waitTime/4f);
        }
    }



    /// <summary>
    /// Fade names to transparent then release text to the pool
    /// </summary>
    /// <param name="level"></param>
    /// <param name="waitTime"></param>
    void FadeNames(int level, float waitTime) 
    {
        foreach (QuadBehaviour quad in GameManager.instance.MapConstructor.Levels[level - 1])
        {
            quad.FadeText(waitTime);
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
