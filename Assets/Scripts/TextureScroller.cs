using UnityEngine;

public class TextureScroller : MonoBehaviour
{
    [SerializeField] private Renderer renderer;
    [SerializeField] private float scrollSpeedX = 0.1f;
    [SerializeField] private float scrollSpeedY = 0.1f;

    private Material material = null;
    private float offsetX = 0;
    private float offsetY = 0;

    private void Start()
    {
        if (renderer == null) 
            renderer = GetComponent<Renderer>();

        if (renderer == null)
        {
            Debug.Log("Cant find Renderer!");
            enabled = false;
            return;
        }

        material = renderer.material;
    }

    private void Update()
    {
        offsetX = Time.time * scrollSpeedX;
        offsetY = Time.time * scrollSpeedY;
        material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
    }
}
