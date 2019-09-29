using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public void FadeOut()
    {
        StartCoroutine("Fade");
    }

    public IEnumerator Fade()
    {
        for (float t = 1.0f; t >= 0.0f; t -= Time.deltaTime)
        {
            Color newColor = new Color(t, t, t, t);
            transform.GetComponent<Image>().color = newColor;
            yield return null;
        }
        Destroy(gameObject);
    }
}
