using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private GameObject winGameUI;
    [SerializeField] private GameObject loseGameUI;

    private void Start()
    {
        winGameUI.SetActive(false);
        loseGameUI.SetActive(false);

        UnitManager.Instance.OnAllAlliesDead += UnitManager_OnAllAlliesDead;
        UnitManager.Instance.OnAllEnemiesDead += UnitManager_OnAllEnemiesDead;
    }

    private void UnitManager_OnAllAlliesDead(object sender, EventArgs e)
    {
        StartCoroutine(SetLoseGameUI());
    }

    private void UnitManager_OnAllEnemiesDead(object sender, EventArgs e)
    {
        StartCoroutine(SetWinGameUI());
    }

    private IEnumerator SetLoseGameUI()
    {
        yield return new WaitForSeconds(1);
        loseGameUI.SetActive(true);
    }

    private IEnumerator SetWinGameUI()
    {
        yield return new WaitForSeconds(1);
        winGameUI.SetActive(true);
    }
}
