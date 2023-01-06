using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceUnitsUI : MonoBehaviour
{
    public void OnPlayButtonClicked()
    {
        Dictionary<Vector2, Transform> cardSlotDataDictionary = MapGrid.Instance.GetCardSlotData();

        foreach (KeyValuePair<Vector2, Transform> cardSlotData in cardSlotDataDictionary)
        {
            GridPosition gridPosition = new GridPosition((int) cardSlotData.Key.x, (int) cardSlotData.Key.y);
            LevelGrid.Instance.SpawnUnitAtGridPosition(gridPosition, cardSlotData.Value);
        }

        gameObject.SetActive(false);
    }
}
