using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartContainer : MonoBehaviour
{
    private bool isAlreadyEnabled=false;
    public void Deactivate()
    {
        if (isAlreadyEnabled)
        {
            StartCoroutine(Fade(0.4f, false));
            isAlreadyEnabled = false;
        }
    }

    public void Activate()
    {
        if (!isAlreadyEnabled)
        {
            GetComponent<Image>().enabled = true;
            StartCoroutine(Fade(0.4f, true));
            isAlreadyEnabled = true;
        }
    }

    IEnumerator Fade(float aTime, bool isFadingIn)
    {
        float alpha = transform.GetComponent<Image>().color.a;
        if(!isFadingIn)
        {
            for (float t = 1.0f; t >= 0.0f; t -= Time.deltaTime / aTime)
            {
                Color newColor = new Color(t, t, t, t);
                transform.GetComponent<Image>().color = newColor;
                yield return null;
            }
            GetComponent<Image>().enabled = false;
        }
        else
        {
            for (float t = 0.0f; t <= 1.0f; t += Time.deltaTime / aTime)
            {
                Color newColor = new Color(t, t, t, t);
                transform.GetComponent<Image>().color = newColor;
                yield return null;
            }
        }
    }
}
