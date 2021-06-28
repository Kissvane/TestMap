using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField]
    Transform Transform;

    [SerializeField]
    Transform LowLimit;

    [SerializeField]
    Transform HighLimit;

    [SerializeField]
    float Speed = 1f;

    Vector3 velocity = Vector3.zero;

    [SerializeField]
    Vector3 target;
    [SerializeField]
    Vector3 move;

    Vector3 lastPosition;

    [SerializeField]
    float minimalDistance = 0.3f;

    float updateTreshhold = 0.1f;

    float currentDistance = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        UpdateTarget();
    }

    public void UpdateTarget()
    {
        target = Transform.position;
    }

    public void SetMoveX(int value)
    {
        move.x = value;
    }

    public void SetMoveY(int value)
    {
        move.y = value;
    }

    public void StopCamera()
    {
        move = Vector3.zero;
        Transform.position = target;
    }

    // Update is called once per frame
    void Update()
    {
#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
        move = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
#endif
        target.z = Transform.position.z;
        //have a constant move speed in all zoom level
        float UsedSpeed = Speed * Linker.instance.MapZoomer.Camera.orthographicSize;

        if (move != Vector3.zero)
        {
            target = Transform.position + move.normalized * UsedSpeed;
            target = new Vector3( Mathf.Clamp(target.x, LowLimit.position.x, HighLimit.position.x), Mathf.Clamp(target.y, LowLimit.position.y, HighLimit.position.y), Transform.position.z);
        }

        //ease in and out on camera movement
        //approximation problem between vectors due to SmoothDamp
        if (Transform.position != target)
        {
            if (Vector3.Distance(Transform.position, target) <= minimalDistance)
            {
                Transform.position = target;
            }
            Transform.position = Vector3.SmoothDamp(Transform.position, target, ref velocity, 0.25f);
            currentDistance += Vector3.Distance(Transform.position, lastPosition); 
            if (currentDistance >= updateTreshhold*Linker.instance.MapZoomer.Camera.orthographicSize) 
            {
                currentDistance = 0f;
                Linker.instance.QuadManager.RecursiveTestVisibility(true);
            }
        }

        lastPosition = Transform.position;
    }
}
