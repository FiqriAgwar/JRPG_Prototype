using UnityEngine;

public enum BasicAttackType
{
    Melee,
    Ranged
}

[CreateAssetMenu(menuName = "Scriptable Objects / Characters")]
public class CharacterData: ScriptableObject
{
    public string charName;

    public Sprite portrait;

    public float baseHP;
    public float speed;
    public float baseAttack;
    public float baseDefend;
    public float baseMana;

    public BasicAttackType attackType;
    public GameObject basicAttackProjectile;
    public float projectileSpeed = 10f;

    public RuntimeAnimatorController animatorController;
    public Sprite idleSprite;

    public SkillData[] skills;
}
