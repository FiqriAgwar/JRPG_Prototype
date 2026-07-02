using System.Collections;
using UnityEngine;

public class RenderableController : MonoBehaviour
{
    public Animator animator { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }

    private bool facingRight;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        facingRight = true;
        spriteRenderer.flipX = !facingRight;
    }

    public void SetVariable(string name, float value)
    {
        animator.SetFloat(name, value);
    }

    public void SetVariable(string name, bool value)
    {
        animator.SetBool(name, value);
    }

    public void SetVariable(string name, int value)
    {
        animator.SetInteger(name, value);
    }

    public void SetVariable(string name)
    {
        animator.SetTrigger(name);
    }

    public void SetFacingOrientation(float direction)
    {
        if (direction > 0 && !facingRight)
        {
            facingRight = true;
        }
        else if (direction < 0 && facingRight)
        {
            facingRight = false;
        }

        spriteRenderer.flipX = !facingRight;
    }

    public void SetHighlight(Color color)
    {
        spriteRenderer.color = color;
    }

    public IEnumerator PlayAnimation(string triggerName)
    {
        var previous = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;

        animator.SetTrigger(triggerName);

        while (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == previous)
        {
            yield return null;
        }

        int entered = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;

        while (
            animator.GetCurrentAnimatorStateInfo(0).fullPathHash == entered &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f
        )
        {
            yield return null;
        }
    }
}
