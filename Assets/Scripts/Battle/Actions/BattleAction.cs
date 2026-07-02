using System.Collections;
using System.Collections.Generic;

public abstract class BattleAction
{
    public BattleUnit attacker;
    public List<BattleUnit> defenders;

    public abstract IEnumerator Execute();
}