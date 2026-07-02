using System.Collections;
using UnityEngine;

public class AttackAction: BattleAction
{
    public override IEnumerator Execute()
    {
        // Animation
        BattleActor attActor = attacker.actor;
        BattleActor defActor = defenders[0].actor;

        if(attacker.charData.attackType == BasicAttackType.Melee)
        {
            yield return ExecuteMeleeAttack(attActor, defActor);
        }
        else
        {
            yield return ExecuteRangedAttack(attActor, defActor);
        }     
    }

    private IEnumerator ExecuteMeleeAttack(BattleActor attActor, BattleActor defActor)
    {
        Vector3 attackPos = Vector3.Lerp(attActor.transform.position, defActor.transform.position, 0.6f);

        yield return attActor.MoveTo(attackPos, 0.15f);

        yield return attActor.renderControl.PlayAnimation("Attack");

        yield return DefFlash();

        yield return new WaitForSeconds(0.2f);

        ResolveDamage();

        yield return new WaitForSeconds(0.15f);

        yield return attActor.Return();
    }

    private IEnumerator ExecuteRangedAttack(BattleActor attActor, BattleActor defActor)
    {
        yield return attActor.renderControl.PlayAnimation("Attack");
        yield return attActor.ShootProjectile(attActor.unit.charData, defActor);

        ResolveDamage();
    }

    private IEnumerator DefFlash()
    {
        for (int i = 0; i < defenders.Count; i++)
        {
            yield return defenders[i].actor.Flash(Color.red);
        }
    }

    private void ResolveDamage()
    {
        // Logic
        foreach (BattleUnit defender in defenders)
        {
            defender.currentHP -= Mathf.Max(attacker.currentAttack - defender.currentDefend, 0);
            defender.currentDefend = Mathf.Max(defender.currentDefend - attacker.currentAttack, 0);

            if (defender.currentDefend == 0)
            {
                defender.isGuarding = false;
            }

            if (defender.currentHP <= 0)
            {
                defender.currentHP = 0;
                defender.isAlive = false;
            }

            Debug.Log(attacker.charData.charName + " -> " + defender.charData.charName);

            if (defender.actor.statsUI != null)
            {
                defender.actor.statsUI.Refresh();
            }
        }
    }
}