using UnityEngine;
using TMPro;

public class BattleMenuOption : MonoBehaviour
{
    public RectTransform rect => (RectTransform)transform;
    public TMP_Text label;

    public BattleMenuOptionData Data { get; private set; }

    public void Bind(BattleMenuOptionData data)
    {
        this.Data = data;
        label.text = data.text;
    }
}
