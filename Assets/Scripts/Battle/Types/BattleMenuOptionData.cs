using Fungus;
using UnityEngine;

public class BattleMenuOptionData
{
    public string text;
    public BattleCommand? command;
    public SkillData skill;
    public bool isBack;

    public BattleMenuOptionData(
        string label,
        BattleCommand? command = null,
        SkillData skill = null,
        bool isBack = false
    )
    {
        this.text = label;
        this.command = command;
        this.skill = skill;
        this.isBack = isBack;
    }
}
