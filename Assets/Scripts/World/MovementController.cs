using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 5f;

    private Transform cameraTransform;

    private InputAction moveAction;
    private Rigidbody rigidbody;

    [SerializeField]
    private RenderableController renderable;

    private Vector3 moveInput;

    private bool canMove;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        rigidbody = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        canMove = false;
    }

    void Update()
    {
        if (!canMove)
        {
            return;
        }

        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        moveInput = new Vector3(moveValue.x, 0, moveValue.y);
    }

    void FixedUpdate()
    {
        if (!canMove) { 
            return;
        }

        rigidbody.linearVelocity =
            new Vector3(
                moveInput.x * movementSpeed,
                rigidbody.linearVelocity.y,
                moveInput.z * movementSpeed
            );

        renderable.SetVariable("IsMoving", moveInput.magnitude > 0);
        renderable.SetFacingOrientation(moveInput.x);
    }

    public void EnableMovement() { canMove = true;  }

    public void DisableMovement() { canMove = false; }

    public void ForceStopMovement()
    {
        rigidbody.linearVelocity = Vector3.zero;
    }

}