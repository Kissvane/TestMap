using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Object used to construct the map
/// </summary>
public class SetupQuadBehaviour : AbstractQuad
{
    #region variables
    [SerializeField]
    Transform _transform;
    public Transform Transform { get => _transform; private set => _transform = value; }
    [SerializeField]
    float _quadShakeStrength = 1f;
    public float QuadShakeStrength { get => _quadShakeStrength; private set => _quadShakeStrength = value; }
    [SerializeField]
    float _textShakeStrength = 1f;
    public float TextShakeStrength { get => _textShakeStrength; private set => _textShakeStrength = value; }
    [SerializeField]
    Text Text;
    [SerializeField]
    string quadName;
    public string QuadName { get => quadName; private set => quadName = value; }
    [SerializeField]
    Vector3 textPosition;
    public Vector3 TextPosition { get => textPosition; private set => textPosition = value; }
    [SerializeField]
    Vector3 textScale;
    public Vector3 TextScale { get => textScale; private set => textScale = value; }
    [SerializeField]
    Vector3 initialPosition;
    [SerializeField]
    private int level;
    public int Level { get => level; private set => level = value; }
    
    [SerializeField]
    SpriteRenderer _renderer;
    public SpriteRenderer Renderer { get => _renderer; private set => _renderer = value; }
    #endregion

    /// <summary>
    /// Initialize datas
    /// </summary>
    /// <param name="toCopy">transform to get position and scale information</param>
    /// <param name="quadName">name of the future quad</param>
    /// <param name="level">level of the future quad</param>
    public override void Initialize(Transform toCopy, string quadName, int level)
    {
        TextPosition = toCopy.localPosition;
        TextScale = toCopy.localScale;
        this.QuadName = quadName;
        initialPosition = Transform.localPosition;
        Level = level;
    }
}
