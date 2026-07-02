using System.Collections.Generic;

public class TurnManager
{
    public List<BattleUnit> turnOrder;

    private int currentIndex;

    public TurnManager(List<BattleUnit> unitList)
    { 
        turnOrder = new List<BattleUnit>(unitList);

        turnOrder.Sort((a, b)=>
            b.charData.speed.CompareTo(a.charData.speed)
        );

        currentIndex = 0;
    }

    public BattleUnit CurrentUnit() { return turnOrder[currentIndex]; }

    public BattleUnit NextTurn()
    {
        currentIndex++;
        if (currentIndex >= turnOrder.Count)
        {
            currentIndex = 0;
        }

        return CurrentUnit();
    }
}
