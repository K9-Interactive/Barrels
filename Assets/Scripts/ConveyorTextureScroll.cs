using UnityEngine;

public class ConveyorTextureScroll : MonoBehaviour
{
    public int materialIndex = 1;

    private Renderer rend;
    private Vector2 offset;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float scrollSpeed = GlobalSettings.Instance != null ? GlobalSettings.Instance.globalScrollSpeed : 0f;

        offset.y -= scrollSpeed * Time.deltaTime;

        if (rend.materials.Length > materialIndex)
        {
            rend.materials[materialIndex].SetTextureOffset("_MainTex", offset);
        }
    }
}