using System;
using System.Numerics;
using UnityEngine.InputSystem;
using UnityEngine;

public class BattleInput
{
    PlayerControls input;

    public event Action Confirm;
    public event Action NavigateRight;
    public event Action NavigateLeft;

    public void Enable()
    {
        input = new();

        input.Enable();

        input.Battle
            .Confirm
            .performed
            +=
            OnConfirm;

        input.Battle
            .Navigate
            .performed
            +=
            OnNavigate;
    }

    void OnConfirm(
        InputAction.CallbackContext _
    )
    {
        Confirm?.Invoke();
    }

    void OnNavigate(
        InputAction.CallbackContext _ctx
    )
    {
        float value = _ctx.ReadValue<float>();

        if (value > 0)
        {
            NavigateRight?.Invoke();
        }

        if (value < 0)
        {
            NavigateLeft?.Invoke();
        }
    }
}