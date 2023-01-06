using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour
{
    public static MapGrid Instance { get; private set; }

    private List<UnitCardSlot> unitCardSlots;

    private int x = 0;
    private int y = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one LevelGrid! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        unitCardSlots = new List<UnitCardSlot>();
    }

    private void Start()
    {
        foreach (UnitCardSlot unitCardSlot in transform.GetComponentsInChildren<UnitCardSlot>())
        {
            if(x == LevelGrid.Instance.GetWidth())
            {
                y++;
                x = 0;
            }

            unitCardSlot.position = new Vector2(x, y);

            unitCardSlots.Add(unitCardSlot);

            x++;
        }
    }

    public Dictionary<Vector2, Transform> GetCardSlotData()
    {
        Dictionary<Vector2, Transform> cardSlotData = new Dictionary<Vector2, Transform>();

        foreach (UnitCardSlot unitCardSlot in unitCardSlots)
        {
            if(unitCardSlot.FirstDragDropCard)
                cardSlotData.Add(unitCardSlot.position, unitCardSlot.FirstDragDropCard.UnitCardPref);
        }

        return cardSlotData;
    }
}
