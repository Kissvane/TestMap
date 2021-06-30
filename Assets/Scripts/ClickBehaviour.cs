using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

/// <summary>
/// Detect on which quad the user clicked and activate necessary feedbacks
/// </summary>
public class ClickBehaviour : MonoBehaviour
{
    [SerializeField]
    Camera Camera;
    [SerializeField]
    LayerMask LayerMask;

    // Update is called once per frame
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
                RaycastHit2D rayHit = rayHits.OrderByDescending(x => x.collider.GetComponent<QuadBehaviour>().Level).First();
                if (Mathf.RoundToInt(rayHit.collider.GetComponent<QuadBehaviour>().Level) == GameManager.instance.MapZoomer.ZoomLevel) 
                {
                    //activate shake feedback
                    rayHit.collider.gameObject.GetComponent<QuadBehaviour>().ClickFeedback();
                    //show the number of visible quad
                    GameManager.instance.QuadCounter.ShowResult();
                }
            }
        }
    }

    /// <summary>
    /// Test if the pointer is on UI object. this one works on mobile platform.
    /// The unity one doesn't
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
