using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UnitCardSlot : MonoBehaviour, IDropHandler
{
    private DragDropCard firstDragDropCard;

    public Vector2 position;

    public void OnDrop(PointerEventData eventData)
    { 
        if(transform.childCount == 0)
        {
            firstDragDropCard = eventData.pointerDrag.GetComponent<DragDropCard>();
            firstDragDropCard.HasDropped = true;
            firstDragDropCard.ParentAfterDrag = transform;
        }
        else
        {
            DragDropCard secondDragDropCard = eventData.pointerDrag.GetComponent<DragDropCard>();
            secondDragDropCard.HasDropped = true;
            secondDragDropCard.ParentAfterDrag = transform;

            firstDragDropCard.HasDropped = false;
            firstDragDropCard.transform.SetParent(firstDragDropCard.InitialParent, true);

            firstDragDropCard = secondDragDropCard;
        }
    }

    public DragDropCard FirstDragDropCard { get { return firstDragDropCard; } }
}
