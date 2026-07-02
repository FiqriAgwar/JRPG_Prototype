using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float stopDistance = 1.5f;

    private bool following;
    private RenderableController renderControl;

    private void Awake()
    {
        renderControl = GetComponentInChildren<RenderableController>();
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void StartFollow()
    {
        following = true;
    }

    public void StopFollow()
    {
        following = false; 
    }

    private void Update()
    {
        if (!following)
        {
            return;
        }

        if (target == null)
        {
            return;            
        }

        Vector3 delta = target.position - transform.position;

        delta.y = 0;

        renderControl.animator.SetBool("IsMoving", (delta.magnitude > stopDistance));

        if (delta.magnitude <= stopDistance)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        renderControl.SetFacingOrientation(delta.x);
    }
}
