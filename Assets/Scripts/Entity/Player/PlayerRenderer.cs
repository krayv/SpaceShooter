using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderer : MonoBehaviour, IRenderer
{
    [SerializeField] float blinkTime = 0.15f;

    private bool isAlreadyBlinked = false;
    private SpriteRenderer[] thisRenderers;

    public void Awake()
    {
        thisRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void Blink()
    {
        if (!isAlreadyBlinked)
        {
            StartCoroutine(BlinkCorountin());
        }
    }

    private IEnumerator BlinkCorountin()
    {
        isAlreadyBlinked = true;
        List<Color> colors = new List<Color>();
        foreach (SpriteRenderer spriteRenderer in thisRenderers)
        {
            Color originalColor = spriteRenderer.color;
            colors.Add(originalColor);
            spriteRenderer.color = new Color(1f, 0.5f, 0.5f, 1f);
        }
        yield return new WaitForSeconds(blinkTime);
        int i = 0;
        foreach (SpriteRenderer spriteRenderer in thisRenderers)
        {
            Color originalColor = colors[i];
            spriteRenderer.color = originalColor;
            i++;
        }

        isAlreadyBlinked = false;
    }
}
