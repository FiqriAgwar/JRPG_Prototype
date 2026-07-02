using System.Collections;
using UnityEngine;

public class DefendAction : BattleAction
{
    public override IEnumerator Execute()
    {
        attacker.isGuarding = true;
        attacker.currentDefend = attacker.charData.baseDefend;

        if (attacker.actor.statsUI != null)
        {
            attacker.actor.statsUI.Refresh();
        }

        yield return null;
    }
}