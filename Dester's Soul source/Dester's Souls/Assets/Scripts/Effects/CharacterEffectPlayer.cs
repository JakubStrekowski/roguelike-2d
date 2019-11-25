using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectPlayer : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Animator animator;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void PlayHurtAnimation()
    {
        spriteRenderer.enabled = true;
        StartCoroutine("AnimateHurtEffect");
    }
    public IEnumerator AnimateHurtEffect()
    {
        animator.Play("HurtEffectPlay");
        yield return new WaitForSeconds(1f);
        spriteRenderer.enabled = false;
    }
}
