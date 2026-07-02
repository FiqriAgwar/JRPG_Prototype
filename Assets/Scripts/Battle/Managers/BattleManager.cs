using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState
{
    TurnStart,
    CommandSelection,
    SkillSelection,
    TargetSelection,
    Execution,
    TurnEnd
}

public enum BattleCommand
{
    Attack, // Limited to attacking a single enemy
    Guard, // Only guard self for now
    Skill
}

public class BattleManager : MonoBehaviour
{
    public BattleConfig battleConfig;

    private BattleInput battleInput;

    private TurnManager turnManager;

    private BattleState battleState;

    [SerializeField] BattleUIManager uiManager;

    List<BattleUnit> allUnit;
    List<BattleUnit> possibleTargets;
    List<BattleActor> playerActors; // for Stats setup
    List<BattleActor> enemyActors; // for Stats setup

    private int commandIndex;
    private int targetIndex;
    private int skillIndex;

    private BattleCommand commandChosen;
    private SkillData skillChosen;

    [SerializeField] private Transform[] playerSpawns;
    [SerializeField] private Transform[] enemySpawns;
    [SerializeField] private GameObject battleActorPrefab;

    private void Start()
    {
        battleInput = new BattleInput();
        battleInput.Enable();
        battleInput.Confirm += OnConfirm;
        battleInput.NavigateLeft += NavigateLeft;
        battleInput.NavigateRight += NavigateRight;

        allUnit = new List<BattleUnit>();
        playerActors = new List<BattleActor>();    
        enemyActors = new List<BattleActor>();    

        SpawnPlayerUnits();
        SpawnEnemyUnits();

        Debug.Log(allUnit.Count);

        turnManager = new TurnManager(allUnit);

        uiManager.SetupStatsUI(playerActors, false);
        uiManager.SetupStatsUI(enemyActors, true);
        uiManager.SetupQueueUI(turnManager.turnOrder);

        StartTurn();
    }

    private void SpawnPlayerUnits()
    {
        for (int i = 0; i < battleConfig.player.Length; i++)
        {
            SpawnUnit(battleConfig.player[i], false, i);
        }
    }
    
    private void SpawnEnemyUnits()
    {
        for (int i = 0; i < battleConfig.enemy.Length; i++)
        {
            SpawnUnit(battleConfig.enemy[i], true, i);
        }
    }

    private void SpawnUnit(CharacterData data, bool isEnemy, int slot)
    {
        BattleUnit unit = new BattleUnit(data, isEnemy);

        if (isEnemy && slot >= enemySpawns.Length)
        {
            return;
        }

        if (!isEnemy && slot >= playerSpawns.Length)
        {
            return;
        }

        Transform spawnPos = isEnemy ? enemySpawns[slot] : playerSpawns[slot];
        GameObject gameObject = Instantiate(battleActorPrefab, spawnPos.position, Quaternion.identity);
        gameObject.name = data.charName;

        BattleActor battleActor = gameObject.GetOrAddComponent<BattleActor>();
        battleActor.Setup(unit);
        unit.actor = battleActor;

        allUnit.Add(unit);
        
        if (!isEnemy)
        {
            playerActors.Add(battleActor);
        }
        else
        {
            enemyActors.Add(battleActor);
        }
    }

    private List<BattleMenuOptionData> GenerateActionMenu(BattleUnit unit)
    {
        return new List<BattleMenuOptionData>()
        {
            new BattleMenuOptionData("Attack", BattleCommand.Attack),
            new BattleMenuOptionData("Skill", BattleCommand.Skill),
            new BattleMenuOptionData("Guard", BattleCommand.Guard),
        };
    }

    private List<BattleMenuOptionData> GenerateSkillMenu(BattleUnit unit)
    {
        List<BattleMenuOptionData> skills = new List<BattleMenuOptionData>();

        foreach(var skill in unit.charData.skills)
        {
            skills.Add(new BattleMenuOptionData(skill.name, skill: skill));
        }

        skills.Add(new BattleMenuOptionData("Back", isBack: true));

        return skills;
    }

    private void StartTurn()
    {
        Debug.Log(turnManager.CurrentUnit().charData.charName + "'s turn");
        battleState = BattleState.TurnStart;

        BattleUnit currentUnit = turnManager.CurrentUnit();

        if (!currentUnit.isAlive)
        {
            EndTurn();
            return;
        }

        currentUnit.actor.queuePortraitUI.SetHighlight(true);

        if (currentUnit.isEnemy)
        {
            ExecuteEnemyTurn();
        }
        else
        {
            uiManager.AttachMenuTo(currentUnit.actor);
            uiManager.OpenMenu(GenerateActionMenu(currentUnit));
            EnterCommandSelection();
        }
    }

    public void EndTurn()
    {
        if (CheckBattleEnd())
        {
            return;
        }

        BattleUnit currentUnit = turnManager.CurrentUnit();
        currentUnit.actor.queuePortraitUI.SetHighlight(false);

        turnManager.NextTurn();

        StartTurn();
    }

    public bool CheckBattleEnd()
    {
        bool allEnemyDead = allUnit.Where(unit => unit.isEnemy).All(enemy => !enemy.isAlive);
        bool allPlayerDead = allUnit.Where(unit => !unit.isEnemy).All(enemy => !enemy.isAlive);

        if (allEnemyDead)
        {
            Debug.Log("Victory");
            SceneManager.LoadScene("End");
            return true;
        }
        else if (allPlayerDead)
        {
            Debug.Log("Defeat");
            uiManager.ShowEndGamePanel();
            return true;
        }

        return false;
    }

    private void ExecuteEnemyTurn()
    {
        SelectCommand(BattleCommand.Attack);
        ConfirmTarget();
    }

    private void OnConfirm()
    {
        Debug.Log("Enter " + ((BattleState)battleState).ToString());
        switch (battleState)
        {
            case BattleState.CommandSelection:
                ConfirmCommand();
                break;
            case BattleState.SkillSelection:
                ConfirmSkill();
                break;
            case BattleState.TargetSelection:
                ConfirmTarget();
                break;
        }
    }

    private IEnumerator ExecuteAction(BattleAction action)
    {
        battleState = BattleState.Execution;
        

        yield return StartCoroutine(
            action.Execute()
        );

        ClearTargets();

        EndTurn();
    }

    private void SelectAttack(BattleUnit currentUnit)
    {
        SelectTarget(currentUnit, TargetType.SingleEnemy);
    }

    private void SelectSkill(BattleUnit currentUnit)
    {
        battleState = BattleState.SkillSelection;
        skillIndex = 0;

        uiManager.OpenMenu(GenerateSkillMenu(currentUnit));
    }

    private void ShowSkill()
    {
        BattleUnit currentUnit = turnManager.CurrentUnit();

        Debug.Log("Skill " + currentUnit.charData.skills[skillIndex].skillName);
    }

    private void ConfirmSkill()
    {
        BattleMenuOptionData selectedSkill = uiManager.CurrentMenu();
        BattleUnit currentUnit = turnManager.CurrentUnit();
        
        if (selectedSkill.isBack)
        {
            EnterCommandSelection();

            uiManager.OpenMenu(
                GenerateActionMenu(currentUnit)
            );

            return;
        }

        skillChosen = selectedSkill.skill;

        SelectTarget(currentUnit, skillChosen.targetType);
    }

    private void GuardSelf(BattleUnit currentUnit)
    {
        ConfirmTarget();
    }

    private void EnterCommandSelection()
    {
        battleState = BattleState.CommandSelection;
        commandIndex = 0;

        ShowCommand();
    }

    private void ShowCommand()
    {
        Debug.Log("Command " + ((BattleCommand)commandIndex).ToString());
    }

    private void ConfirmCommand()
    {
        BattleMenuOptionData selectedCommand = uiManager.CurrentMenu();

        SelectCommand(selectedCommand.command.Value);
    }

    private void SelectCommand(BattleCommand command)
    {
        commandChosen = command;

        BattleUnit currentUnit = turnManager.CurrentUnit();

        switch (command)
        {
            case BattleCommand.Attack:
                SelectAttack(currentUnit);
                break;

            case BattleCommand.Skill:
                SelectSkill(currentUnit);
                break;

            case BattleCommand.Guard:
                GuardSelf(currentUnit);
                break;
        }
    }

    private bool IsTargetNeeded(TargetType targetType)
    {
        switch (targetType)
        {
            case TargetType.SingleEnemy:

            case TargetType.SingleAlly:
                return true;


            default:
                return false;
        }
    }

    private void SelectTarget(BattleUnit currentUnit, TargetType targetType)
    {
        battleState = BattleState.TargetSelection;
        possibleTargets = GetPossibleTargets(currentUnit, targetType);

        if (!IsTargetNeeded(targetType))
        {
            ConfirmTarget();
            return;
        }

        targetIndex = 0;

        ShowTarget();
    }

    private List<BattleUnit> GetPossibleTargets(BattleUnit currentUnit, TargetType targetType)
    {
        switch(targetType)
        {
            case TargetType.Self:
                return new List<BattleUnit>() { currentUnit };
            case TargetType.SingleEnemy:
                return allUnit.Where(x => x.isEnemy != currentUnit.isEnemy).Where(x => x.isAlive).ToList();
            case TargetType.AllEnemy:
                return allUnit.Where(x => x.isEnemy != currentUnit.isEnemy).Where(x => x.isAlive).ToList();
            case TargetType.SingleAlly:
                return allUnit.Where(x => x.isEnemy == currentUnit.isEnemy).Where(x => x.isAlive).ToList();
            case TargetType.AllAlly:
                return allUnit.Where(x => x.isEnemy == currentUnit.isEnemy).Where(x => x.isAlive).ToList();
        }

        return null;
    }

    private void ShowTarget()
    {
        foreach (BattleUnit unit in allUnit)
        {
            unit.actor.SetTargeted(false);
        }

        possibleTargets[targetIndex].actor.SetTargeted(true);
    }

    private void NavigateRight()
    {
        Debug.Log("NavRight");
        switch (battleState)
        {
            case BattleState.TargetSelection:
                NextTarget();
                break;
        }

        uiManager.NavigateMenu(1);
    }

    private void NavigateLeft()
    {
        Debug.Log("NavLeft");
        switch (battleState)
        {
            case BattleState.TargetSelection:
                PrevTarget();
                break;
        }

        uiManager.NavigateMenu(-1);
    }

    private void NextTarget()
    {
        targetIndex++;

        if (targetIndex >= possibleTargets.Count)
        {
            targetIndex = 0;
        }

        ShowTarget();
    }
    
    private void PrevTarget()
    {
        if (battleState != BattleState.TargetSelection)
        {
            return;
        }

        targetIndex--;

        if (targetIndex < 0)
        {
            targetIndex = possibleTargets.Count - 1;
        }

        ShowTarget();
    }

    private void ConfirmTarget()
    {
        BattleAction action = BuildAction();

        uiManager.CloseMenu();

        StartCoroutine(ExecuteAction(action));
    }

    private void ClearTargets()
    {
        foreach(BattleUnit unit in allUnit)
        {
            unit.actor.SetTargeted(false);
        }
    }

    private BattleAction BuildAction()
    {
        BattleUnit currentUnit = turnManager.CurrentUnit();

        switch(commandChosen)
        {
            case BattleCommand.Attack:
                return new AttackAction { attacker = currentUnit, defenders = GetSelectedTargets() };
            case BattleCommand.Skill:
                return new SkillAction { attacker = currentUnit, defenders = GetSelectedTargets(), skill = skillChosen };
            case BattleCommand.Guard:
                return new DefendAction { attacker = currentUnit, defenders = null };
        }

        return null;
    }

    private List<BattleUnit> GetSelectedTargets()
    {
        if (possibleTargets.Count == 1)
            return possibleTargets;

        return new List<BattleUnit>()
            {
                possibleTargets[targetIndex]
            };
    }
}
