using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UnitCardSlot : MonoBehaviour, IDropHandler
{
    public static event EventHandler<OnAnyDropToSlotEventArgs> OnAnyDropToSlot;

    public class OnAnyDropToSlotEventArgs : EventArgs
    {
        public RectTransform rectTransformCard;
        public RectTransform rectTransformSlot;
    }

    public void OnDrop(PointerEventData eventData)
    { 
        if(eventData.pointerDrag != null)
        {
            OnAnyDropToSlot?.Invoke(this, new OnAnyDropToSlotEventArgs
            {
                rectTransformCard = eventData.pointerDrag.GetComponent<RectTransform>(),
                rectTransformSlot = GetComponent<RectTransform>()
            });
        }
    }
}
