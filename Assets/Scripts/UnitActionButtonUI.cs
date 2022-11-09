using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitActionButtonUI : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    private Button button;
    private GameObject selectedVisual;
    private BaseAction baseAction;

    private void Awake()
    {
        textMeshPro = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        button = GetComponent<Button>();
        selectedVisual = transform.GetChild(1).gameObject;
    }

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;

        textMeshPro.text = baseAction.GetActionName().ToUpper();

        button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SelectedAction = baseAction;
        });
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.SelectedAction;
        selectedVisual.gameObject.SetActive(selectedBaseAction == baseAction);
    }
}
