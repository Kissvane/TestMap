using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

/// <summary>
/// Activate click feedback when clicking on a quad
/// </summary>
public class ClickBehaviour : MonoBehaviour
{
    [SerializeField]
    LayerMask LayerMask;

    /// <summary>
    /// Detect the click and activate feedback on clicked quad
    /// </summary>
    void Update()
    {
#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !IsPointerOverUIObject())      
#endif
        {
            RaycastHit2D[] rayHits = Physics2D.GetRayIntersectionAll(Camera.main.ScreenPointToRay(Input.mousePosition), 1000f, LayerMask);
            if (rayHits.Length > 0)
            {
                //Debug.Log(rayHits[0].collider.name);
                RaycastHit2D rayHit = rayHits.OrderByDescending(x => x.collider.GetComponent<QuadBehaviour>().QuadData.Level).First();
                if (Mathf.RoundToInt(rayHit.collider.GetComponent<QuadBehaviour>().QuadData.Level) == GameManager.instance.MapZoomer.ZoomLevel) 
                {
                    rayHit.collider.gameObject.GetComponent<QuadBehaviour>().ClickFeedback();
                    GameManager.instance.QuadCounter.ShowResult();
                }
            }
        }
    }

    /// <summary>
    /// The unity methods doesn't work on mobile so I use this one
    /// </summary>
    /// <returns></returns>
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}
