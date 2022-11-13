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
    private GameObject enemyTurnVisual;

    private void Awake()
    {
        endTurn = transform.GetChild(0).GetComponent<Button>();
        turnNumber = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        enemyTurnVisual = transform.GetChild(2).GetComponent<Transform>().gameObject;
    }

    private void Start()
    {
        endTurn.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void UpdateTurnText()
    {
        turnNumber.text = $"TURN {TurnSystem.Instance.TurnNumber}";
    }

    private void UpdateEnemyTurnVisual()
    {
        enemyTurnVisual.SetActive(!TurnSystem.Instance.IsPlayerTurn);
    }

    private void UpdateEndTurnButtonVisibility()
    {
        endTurn.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn);
    }
}
