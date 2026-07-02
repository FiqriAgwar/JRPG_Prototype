using UnityEngine;


public class BattleUnit
{
    public float currentHP;
    public float currentAttack;
    public float currentDefend;
    public float currentMana;

    public bool isAlive;
    public bool isEnemy;

    public bool isGuarding;

    public CharacterData charData;
    public BattleActor actor;

    public BattleUnit(CharacterData data, bool isEnemy)
    {
        this.charData = data;
        this.currentHP = data.baseHP;
        this.currentAttack = data.baseAttack;
        this.currentDefend = data.baseDefend;
        this.currentMana = data.baseMana;
        this.isAlive = true;
        this.isEnemy = isEnemy; 
        this.isGuarding = false;
    }
}
