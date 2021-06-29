using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Calculate and show FPS
/// </summary>
public class FpsCounter : MonoBehaviour
{
    #region variables
    [SerializeField] private Text _fpsText;
    [SerializeField] private float _hudRefreshRate = 1f;
    float _timer;
    #endregion

    private void Update()
    {
        if (Time.unscaledTime > _timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            _fpsText.text = fps.ToString("0");
            _timer = Time.unscaledTime + _hudRefreshRate;
        }
    }
}