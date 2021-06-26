using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class ClickBehaviour : MonoBehaviour
{
    [SerializeField]
    Camera Camera;
    [SerializeField]
    LayerMask LayerMask;
    [SerializeField]
    MapZoomer MapZoomer;

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
                RaycastHit2D rayHit = rayHits.OrderByDescending(x => x.collider.transform.position.z).First();
                if (Mathf.RoundToInt(rayHit.collider.transform.position.z) == MapZoomer.ZoomLevel-1) 
                {
                    rayHit.collider.gameObject.GetComponent<QuadBehaviour>().ClickFeedback();
                }
            }
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}
