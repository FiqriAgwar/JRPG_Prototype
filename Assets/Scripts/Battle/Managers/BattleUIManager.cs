using System.Collections.Generic;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    [Header("Actor and Stats UI")]
    [SerializeField] BattleActorUI battleActorUI;
    [SerializeField] Transform playerStatsContainer;
    [SerializeField] Transform enemyStatsContainer;

    [Header("Queue UI")]
    [SerializeField] BattleQueueUIController queueIconUI;
    [SerializeField] Transform queueIconUIContainer;

    [Header("Action Menu")]
    [SerializeField] BattleActionMenuController actionMenuController;

    [Header("End Game Panel")]
    [SerializeField] GameObject endGamePanel;

    private void Start()
    {
        endGamePanel.SetActive(false);
    }

    public void SetupStatsUI(List<BattleActor> actors, bool isEnemy)
    {
        foreach(var actor in actors)
        {
            var ui = Instantiate(battleActorUI, isEnemy ? enemyStatsContainer : playerStatsContainer);
            ui.Bind(actor);
            actor.statsUI = ui;
        }
    }

    public void SetupQueueUI(List<BattleUnit> unitQueue)
    {
        foreach(var unit in unitQueue)
        {
            var portraitIcon = Instantiate(queueIconUI, queueIconUIContainer);
            portraitIcon.Bind(unit.actor);
            unit.actor.queuePortraitUI = portraitIcon;
        }
    }

    public void AttachMenuTo(BattleActor actor)
    {
        actionMenuController.AttachTo(actor);
    }

    public void OpenMenu(List<BattleMenuOptionData> options)
    {
        actionMenuController.Show(options);
    }
    
    public void CloseMenu()
    {
        actionMenuController.CloseMenu();
    }

    public void NavigateMenu(int dir)
    {
        actionMenuController.Move(dir);
    }

    public BattleMenuOptionData CurrentMenu()
    {
        return actionMenuController.Current.Data;
    }

    public void ShowEndGamePanel()
    {
        endGamePanel.SetActive(true);
    }
}
