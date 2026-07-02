using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleActionMenuController : MonoBehaviour
{
    private RectTransform menu;
    private Camera mainCam;
    private CanvasGroup canvasGroup;

    [SerializeField] private GameObject menuOptionPrefab;
    [SerializeField] private Transform menuOptionContainer;
    [SerializeField] private RectTransform highlighter;
    private List<BattleMenuOption> options;
    private int indexSelected;

    private void Awake()
    {
        mainCam = Camera.main;
        menu = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        options = new List<BattleMenuOption>();
        CloseMenu();
    }

    public BattleMenuOption Current => options[indexSelected];

    public void AttachTo(BattleActor actor)
    {
        Vector2 screenPos = mainCam.WorldToScreenPoint(actor.MenuAnchor.position);

        menu.position = screenPos;
        Refresh();
    }

    private void Refresh()
    {
        if (options.Count == 0)
        {
            return;
        }

        RectTransform selected = options[indexSelected].rect;

        highlighter.localPosition = selected.localPosition;
    }

    public void Move(int direction)
    {
        indexSelected += direction;

        if (indexSelected >= options.Count)
        {
            indexSelected = 0;
        }
        else if (indexSelected < 0)
        {
            indexSelected = options.Count - 1;
        }

        Refresh();
    }

    private void Clear()
    {
        foreach(var obj in options){
            DestroyImmediate(obj.gameObject);
        }

        options.Clear();
    }

    public void Show(List<BattleMenuOptionData> menuData)
    {
        Clear();

        foreach(var data in menuData)
        {
            GameObject optionObject = Instantiate(menuOptionPrefab, menuOptionContainer);
            BattleMenuOption option = optionObject.GetOrAddComponent<BattleMenuOption>();

            option.Bind(data);
            options.Add(option);
        }

        Debug.Log("Total Options" + options.Count);

        indexSelected = 0;

        Canvas.ForceUpdateCanvases();

        Refresh();
        OpenMenu();
    }

    public void OpenMenu()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    
    public void CloseMenu()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
