using UnityEngine;

/// <summary>
/// Manage camera movement
/// </summary>
public class MoveCamera : MonoBehaviour
{
    #region variables
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
    [SerializeField]
    float minimalDistance = 0.1f;
    #endregion

    //initialize target value
    public void UpdateTarget()
    {
        target = Transform.position;
    }

    #region mobile UI functions
    public void SetMoveX(int value)
    {
        move.x = value;
    }

    public void SetMoveY(int value)
    {
        move.y = value;
    }
    #endregion

    void Update()
    {
#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
        move = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
#endif
        target.z = Transform.position.z;
        //have a constant move speed in all zoom level
        float UsedSpeed = Speed * GameManager.instance.MapZoomer.Camera.orthographicSize;

        if (move != Vector3.zero)
        {
            target = Transform.position + move.normalized * UsedSpeed;
            target = new Vector3( Mathf.Clamp(target.x, LowLimit.position.x, HighLimit.position.x), Mathf.Clamp(target.y, LowLimit.position.y, HighLimit.position.y), Transform.position.z);
        }

        //ease in and out on camera movement
        //approximation problem between vectors due to SmoothDamp
        if (Transform.position != target)
        {
            if (Vector3.Distance(Transform.position, target) <= minimalDistance * GameManager.instance.MapZoomer.Camera.orthographicSize)
            {
                Transform.position = target;
            }
            Transform.position = Vector3.SmoothDamp(Transform.position, target, ref velocity, 0.25f);
        }
    }
}
