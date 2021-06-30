using UnityEngine;

/// <summary>
/// Manage horizontal and vertical camera movement
/// </summary>
public class MoveCamera : MonoBehaviour
{
    #region variable
    [SerializeField]
    Transform Transform;

    [SerializeField]
    Transform LowLimit;

    [SerializeField]
    Transform HighLimit;

    [SerializeField]
    float Speed = 1f;

    Vector3 velocity = Vector3.zero;

    Vector3 target;
    [SerializeField]
    Vector3 move;
    #endregion

    #region Monobehaviour callbacks
    void Awake()
    {
        target = Transform.position;
    }

    // Update is called once per frame
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
            target = new Vector3(Mathf.Clamp(target.x, LowLimit.position.x, HighLimit.position.x), Mathf.Clamp(target.y, LowLimit.position.y, HighLimit.position.y), Transform.position.z);
        }

        //ease in and out on camera movement
        if (Transform.position != target)
        {
            Transform.position = Vector3.SmoothDamp(Transform.position, target, ref velocity, 0.25f);
        }
    }
    #endregion

    #region UI move functions
    public void SetMoveX(int value)
    {
        move.x = value;
    }

    public void SetMoveY(int value)
    {
        move.y = value;
    }
    #endregion
}
