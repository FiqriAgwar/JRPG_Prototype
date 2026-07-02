using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleActorUI : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text hpText;
    [SerializeField] TMP_Text manaText;
    [SerializeField] TMP_Text guardText;

    [SerializeField] Image hpBar;
    [SerializeField] Image manaBar;
    [SerializeField] Image guardBar;

    BattleActor actor;

    public void Bind(BattleActor actor)
    {
        this.actor = actor;

        Refresh();
    }

    public void Refresh()
    {
        nameText.text = actor.unit.charData.name;
        
        hpText.text = $"{actor.unit.currentHP} / {actor.unit.charData.baseHP}";
        hpBar.fillAmount = actor.unit.charData.baseHP > 0 ? (float)actor.unit.currentHP / actor.unit.charData.baseHP : 0;
        manaText.text = $"{actor.unit.currentMana} / {actor.unit.charData.baseMana}";
        manaBar.fillAmount = actor.unit.charData.baseMana > 0 ? (float)actor.unit.currentMana / actor.unit.charData.baseMana : 0;
        guardText.text = $"{actor.unit.currentDefend} / {actor.unit.charData.baseDefend}";
        guardBar.fillAmount = actor.unit.charData.baseDefend > 0 ? (float)actor.unit.currentDefend / actor.unit.charData.baseDefend : 0;
    }
}
