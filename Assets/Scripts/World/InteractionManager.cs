using UnityEngine;
using TMPro;

public class InteractionManager : MonoBehaviour
{
    private IInteractable interactable;
    PlayerControls input;
    [SerializeField] private TMP_Text interactionHelper;

    private void Awake()
    {
        input = new PlayerControls();
        interactionHelper.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (input.Player.Interact.WasPressedThisFrame())
        {
            interactable?.Interact();
            interactionHelper?.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        Debug.Log("Interaction Enabled");
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        interactable = other.GetComponent<IInteractable>();

        if (interactable != null)
        {
            interactionHelper.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        interactable = null;

        interactionHelper.gameObject.SetActive(false);
    }
}
