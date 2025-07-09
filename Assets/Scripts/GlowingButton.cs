using UnityEngine;
using System;

public class GlowingButton : MonoBehaviour
{
    [ColorUsage(true, true)]
    public Color glowColor = Color.white;

    [Range(0f, 5f)]
    public float glowIntensity = 1f;

    public bool isButtonActive = false;

    private Renderer rend;

    public Action onMouseDown;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (isButtonActive)
        {
            ApplyGlow();
        }
        else
        {
            RemoveGlow();
        }
    }

    private void OnMouseDown()
    {
        onMouseDown?.Invoke(); 
    }

    public void SetButtonActive(bool active)
    {
        isButtonActive = active;
        if (active)
        {
            ApplyGlow();
        }
        else
        {
            RemoveGlow();
        }
    }

    private void ApplyGlow()
    {
        if (rend != null && rend.material.HasProperty("_EmissionColor"))
        {
            rend.material.EnableKeyword("_EMISSION");
            rend.material.SetColor("_EmissionColor", glowColor * glowIntensity);
        }
    }

    private void RemoveGlow()
    {
        if (rend != null && rend.material.HasProperty("_EmissionColor"))
        {
            rend.material.SetColor("_EmissionColor", Color.black);
        }
    }
}