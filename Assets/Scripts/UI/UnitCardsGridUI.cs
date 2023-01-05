using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCardsGridUI : MonoBehaviour
{
    void Start()
    {
        List<UnitCard> unitCards = new List<UnitCard>();
        unitCards = AccountUnitsManager.Instance.GetUnitsOnAccount();

        foreach (UnitCard unitCard in unitCards)
        {
            GameObject unitCardInstance = new GameObject();
            Image image = unitCardInstance.AddComponent<Image>();

            image.sprite = unitCard.sprite;

            unitCardInstance.GetComponent<RectTransform>().SetParent(transform);
            unitCardInstance.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
        }
    }
}
