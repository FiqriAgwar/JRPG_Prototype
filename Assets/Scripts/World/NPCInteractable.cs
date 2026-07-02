using Fungus;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [Header("Interaction Flowchart")]
    public Flowchart flowchart;
    public string blockName;
    private bool interacted;
    public bool canInteract = false;
    private bool playerNearby = false;

    public void Interact()
    {
        if (!canInteract)
        {
            return;
        }

        if (interacted)
        {
            return;
        }

        if (!playerNearby)
        {
            return;
        }

        interacted = true;

        flowchart.ExecuteBlock(blockName);
    }

    public void EnableInteraction()
    {
        canInteract = true;
    }
    
    public void DisableInteraction()
    {
        canInteract = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}
