using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadingAnim : MonoBehaviour
{

    public void Start()
    {
        StartCoroutine("AnimateElement");
    }
    public Sprite[] anim;

    int iter = 0;

    public IEnumerator AnimateElement()
    {
        while (true)
        {
            GetComponent<Image>().sprite = anim[iter];
            iter=(iter+1)%anim.Length;
            yield return new WaitForSeconds(0.4f);
        }
        
    }
}
