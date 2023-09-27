using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Background : MonoBehaviour
{
    [SerializeField] private Color winColor;
    [SerializeField] private float winColorChangeTime;

    private Color defaultColor;
    private SpriteRenderer spriteRenderer;

    private IEnumerator ColorChange()
    {
        float timer = 0;
        Color deltaColor = winColor - defaultColor;
        while (timer < winColorChangeTime)
        {
            timer += Time.deltaTime;
            spriteRenderer.color = defaultColor + deltaColor * (timer / winColorChangeTime);
            yield return new WaitForEndOfFrame();
        }

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            spriteRenderer.color = defaultColor + deltaColor * (timer / winColorChangeTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public void ShowWin()
    {
        StartCoroutine(ColorChange());
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
    }
}
