using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuadBehaviour : MonoBehaviour
{
    [SerializeField]
    Transform Transform;
    [SerializeField]
    float ShakeStrength = 1f;
    [SerializeField]
    Collider2D Collider;
    [SerializeField]
    Text Text;
    [SerializeField]
    string quadName;
    [SerializeField]
    Vector3 textPosition;
    [SerializeField]
    Vector3 textScale;
    [SerializeField]
    Vector3 initialPosition;
    [SerializeField]
    private int level;
    public int Level { get => level; private set => level = value; }
    

    
    [SerializeField]
    Renderer _renderer;
    public Renderer Renderer { get => _renderer; private set => _renderer = value; }
    bool isShaking = false;

    [SerializeField]
    Canvas NameCanvas;

    
    public Renderer GetRenderer()
    {
        return Renderer;
    }

    void OnBecameVisible()
    {
        if (Linker.instance.MapZoomer.ZoomLevel == level) 
        {
            GetFromPool();
        }
    }

    void OnBecameInvisible()
    {
        GiveToPool();
    }

    void OnDisable()
    {
        GiveToPool();
        if (isShaking)
        {
            DOTween.Kill(Transform);
            ResetQuadPosition();
        }
    }

    public void DisableText()
    {
        GiveToPool();
    }

    void GetFromPool()
    {
        //Get Text from object pooler
        Text = Linker.instance.Pool.Pull();
        //set the wanted scale and position
        Transform textTransform = Text.transform;
        textTransform.localPosition = textPosition;
        textTransform.localScale = textScale;
        Text.text = quadName;
        Text.gameObject.SetActive(true);
    }

    void GiveToPool()
    {
        if (Text != null) 
        {
            //release the text to the object pooler
            Linker.instance.Pool.Release(Text);
            //disable it
            Text = null;
        }
    }

    public void Initialize(Transform toCopy, string quadName, int level)
    {
        textPosition = toCopy.localPosition;
        textScale = toCopy.localScale;
        this.quadName = quadName;
        initialPosition = Transform.localPosition;
        Level = level;
    }

    public void EnableCollider(bool activate)
    {
        Collider.enabled = activate;
    }

    public void ClickFeedback()
    {
        DOTween.Kill(Transform);
        Transform textTransform = Text.transform;
        DOTween.Kill(textTransform.transform);
        ResetQuadPosition();
        ResetTextPosition();
        isShaking = true;
        //shake the Quad
        textTransform.DOShakePosition(0.5f, textTransform.lossyScale.x * ShakeStrength, 20, 0).OnComplete(ResetTextPosition);
        Transform.DOShakePosition(0.5f, Transform.lossyScale.x * ShakeStrength, 20, 0).OnComplete(ResetQuadPosition);
        Linker.instance.QuadCounter.ShowResult();
    }

    void ResetTextPosition()
    {
        isShaking = false;
        Text.transform.localPosition = textPosition;
    }

    void ResetQuadPosition()
    {
        Transform.localPosition = initialPosition;
    }
}
