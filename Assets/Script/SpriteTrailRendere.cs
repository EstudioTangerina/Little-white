using UnityEngine;

public class SpriteGhostTrailRenderer : MonoBehaviour
{
    public Color[] colors = new Color[4]; // Array to hold the four colors
    public bool enableOnAwake = true;
    public bool singleColorShader = true;
    public float updateInterval = 0.1f;
    public int ghosts = 4;
    public int ghostLayerOrder = 0; // Base layer order for ghosts
    public SpriteRenderer[] spriteRenderers; // Array of SpriteRenderers

    private float timer;
    private int ghostCount;

    private void Awake()
    {
        if (enableOnAwake)
        {
            enabled = true;
        }
        else
        {
            enabled = false;
        }

        if (spriteRenderers == null || spriteRenderers.Length == 0)
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        }
    }

    private void OnEnable()
    {
        timer = 0;
        ghostCount = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            CreateGhosts();
            timer = 0;
        }
    }

    private void CreateGhosts()
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            GameObject ghost = new GameObject("Ghost");
            SpriteRenderer ghostRenderer = ghost.AddComponent<SpriteRenderer>();
            ghostRenderer.sprite = spriteRenderer.sprite;

            // Interpolate color based on ghostCount
            float t = (float)ghostCount / (ghosts - 1);
            Color interpolatedColor = InterpolateColors(colors, t);
            ghostRenderer.color = new Color(interpolatedColor.r, interpolatedColor.g, interpolatedColor.b, 0.5f); // Semi-transparent
            ghostRenderer.flipX = spriteRenderer.flipX; // Apply the flipping state

            // Set layer order
            ghostRenderer.sortingOrder = ghostLayerOrder;

            ghost.transform.position = spriteRenderer.transform.position;
            ghost.transform.rotation = spriteRenderer.transform.rotation;
            ghost.transform.localScale = spriteRenderer.transform.localScale;

            Destroy(ghost, ghosts * updateInterval); // Destroy after a certain time
        }

        ghostCount = (ghostCount + 1) % ghosts; // Cycle ghostCount to create a looping gradient effect
    }

    private Color InterpolateColors(Color[] colors, float t)
    {
        if (colors.Length == 0)
        {
            return Color.white;
        }

        if (colors.Length == 1)
        {
            return colors[0];
        }

        int lastIndex = colors.Length - 1;
        float scaledT = t * lastIndex;
        int index = Mathf.FloorToInt(scaledT);
        float lerpValue = scaledT - index;

        if (index >= lastIndex)
        {
            return colors[lastIndex];
        }

        return Color.Lerp(colors[index], colors[index + 1], lerpValue);
    }
}
