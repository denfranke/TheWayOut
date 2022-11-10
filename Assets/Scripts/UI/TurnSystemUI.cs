using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TurnSystemUI : MonoBehaviour
{
    private Button endTurn;
    private TextMeshProUGUI turnNumber;

    private void Awake()
    {
        endTurn = transform.GetChild(0).GetComponent<Button>();
        turnNumber = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        endTurn.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        UpdateTurnText();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
    }

    private void UpdateTurnText()
    {
        turnNumber.text = $"TURN {TurnSystem.Instance.TurnNumber}";
    }
}
