using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenEffects : MonoBehaviour
{
    public Image heroHitSprite;
    public Image herohealedSprite;
    public Image heroLowHealthSprite;
    // Start is called before the first frame update


    public void PlayHealEffect()
    {
        StartCoroutine(Fade(herohealedSprite));
    }

    public void PlayHitEffect()
    {
        StartCoroutine(Fade(heroHitSprite));
    }

    public void StartHeroLowHealthEffect()
    {
        StartCoroutine(FadeInOut(heroLowHealthSprite));
    }

    public void StopHeroLowHealthEffect()
    {
        if (heroLowHealthSprite.enabled)
        {
            StopCoroutine(FadeInOut(heroLowHealthSprite));
            heroLowHealthSprite.enabled = false;
        }
    }

    private IEnumerator Fade(Image spriteRender)
    {
        spriteRender.enabled = true;
        for (float t = 0.45f; t >= 0.0f; t -= Time.deltaTime)
        {
            Color newColor = new Color(spriteRender.color.r, spriteRender.color.g, spriteRender.color.b, t);
            spriteRender.color = newColor;
            yield return null;
        }
        spriteRender.enabled = false;
    }

    private IEnumerator FadeInOut(Image spriteRender)
    {
        spriteRender.enabled = true;
        bool isFadingOut = true;
        while (true)
        {
            if (isFadingOut)
            {
                for (float t = 0.3f; t >= 0.0f; t -= Time.deltaTime * 0.2f)
                {
                    Color newColor = new Color(spriteRender.color.r, spriteRender.color.g, spriteRender.color.b, t);
                    spriteRender.color = newColor;
                    yield return null;
                }
                isFadingOut = !isFadingOut;
            }
            else
            {
                for (float t = 0.0f; t <= 0.3f; t += Time.deltaTime * 0.2f)
                {
                    Color newColor = new Color(spriteRender.color.r, spriteRender.color.g, spriteRender.color.b, t);
                    spriteRender.color = newColor;
                    yield return null;
                }
                isFadingOut = !isFadingOut;
            }
        }
    }
}
