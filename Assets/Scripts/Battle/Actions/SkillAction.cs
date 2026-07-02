using System.Collections;
using UnityEngine;

public class SkillAction: BattleAction
{
    public SkillData skill;

    public override IEnumerator Execute()
    {
        if (skill.manaNeed > attacker.currentMana)
        {
            Debug.Log("Not Enough Mana");
            yield return null;
        }

        foreach (BattleUnit defender in defenders)
        {
            defender.currentHP -= Mathf.Max(skill.powerMultiplier * attacker.currentAttack - defender.currentDefend, 0);
            if (defender.currentHP < 0)
            {
                defender.currentHP = 0;
                defender.isAlive = false;
            }

            Debug.Log(attacker.charData.charName + " -> " + defender.charData.charName);
        }

        attacker.currentMana -= skill.manaNeed;

        yield return null;
    }
}
