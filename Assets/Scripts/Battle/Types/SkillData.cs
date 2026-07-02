using UnityEngine;

public enum TargetType
{
    SingleEnemy,
    AllEnemy,
    SingleAlly,
    AllAlly,
    Self
}

[CreateAssetMenu(menuName = "Scriptable Objects / Skill")]
public class SkillData: ScriptableObject
{
    public string skillName;

    public float powerMultiplier;
    public float manaNeed;

    public TargetType targetType;
    public AnimationClip animation;
}
