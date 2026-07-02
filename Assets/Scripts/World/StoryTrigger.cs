using UnityEngine;
using Fungus;

public class StoryTrigger : MonoBehaviour
{
    public Flowchart flowchart;

    public string blockName;

    bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        triggered = true;

        flowchart.ExecuteBlock(blockName);
    }
}
