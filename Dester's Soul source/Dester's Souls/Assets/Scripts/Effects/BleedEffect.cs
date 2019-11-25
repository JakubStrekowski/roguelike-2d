using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfterTime());
        GetComponent<ParticleSystem>().Play();
    }

    public IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
}
