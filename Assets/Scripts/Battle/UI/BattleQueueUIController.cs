using UnityEngine;
using UnityEngine.UI;

public class BattleQueueUIController : MonoBehaviour
{
    [SerializeField] Image portrait;
    [SerializeField] Image highlight;

    public void Bind(BattleActor actor)
    {
        portrait.sprite = actor.unit.charData.portrait;
        highlight.gameObject.SetActive(false);
    }

    public void SetHighlight(bool isActive)
    {
        highlight.gameObject.SetActive(isActive); 
    }
}
