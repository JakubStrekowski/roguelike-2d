using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeartContainer : MonoBehaviour
{
    private bool isAlreadyEnabled = false;

    public void Deactivate()
    {
        if (isAlreadyEnabled)
        {
            StartCoroutine(Fade(0.4f, false));
            isAlreadyEnabled = false;
        }
    }

    public void DeactivateFast()
    {
        if (isAlreadyEnabled)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            isAlreadyEnabled = false;
        }
    }

    public void Activate()
    {
        if (!isAlreadyEnabled)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            isAlreadyEnabled = true;
        }
    }

    IEnumerator Fade(float aTime, bool isFadingIn)
    {
        float alpha = transform.GetComponent<SpriteRenderer>().color.a;
        if (!isFadingIn)
        {
            for (float t = 1.0f; t >= 0.0f; t -= Time.deltaTime / aTime)
            {
                Color newColor = new Color(t, t, t, t);
                transform.GetComponent<SpriteRenderer>().color = newColor;
                yield return null;
            }
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
