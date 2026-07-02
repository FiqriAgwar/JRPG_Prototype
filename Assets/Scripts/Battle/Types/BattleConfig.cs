using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects / Battle Configuration")]
public class BattleConfig: ScriptableObject
{
    public CharacterData[] player;
    public CharacterData[] enemy;
}
