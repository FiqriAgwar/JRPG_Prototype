using Fungus;
using UnityEngine;

public class AutoInteract : MonoBehaviour, IInteractable
{
    [Header("Interaction Flowchart")]
    public Flowchart flowchart;
    public string blockName;
    private bool interacted;

    public void Interact()
    {
        if (interacted)
        {
            return;
        }

        interacted = true;

        flowchart.ExecuteBlock(blockName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Interact();
        }
    }
}
