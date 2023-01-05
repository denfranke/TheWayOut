using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionBusyUI : MonoBehaviour
{
    private void Start()
    {
        UnitActionSystem.Instance.OnBusyActionChanged += UnitActionSystem_OnBusyActionChanged;

        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void UnitActionSystem_OnBusyActionChanged(object sender, bool isBusy)
    {
        if (isBusy)
            Show();
        else
            Hide();
    }
}
