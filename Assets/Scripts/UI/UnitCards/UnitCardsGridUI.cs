using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCardsGridUI : MonoBehaviour
{
    [SerializeField] private Transform unitCardPref;

    void Start()
    {
        List<UnitCard> unitCards = new List<UnitCard>();
        unitCards = AccountUnitsManager.Instance.GetUnitsOnAccount();

        foreach (UnitCard unitCard in unitCards)
        {
            GameObject unitCardSlotInstance = Instantiate(unitCardPref, transform).gameObject;

            Image image = unitCardSlotInstance.transform.GetChild(0).GetComponent<Image>();
            image.sprite = unitCard.sprite;
        }
    }
}
