using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private bool hasSlot = false;
    private bool hasDropped = false;

    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private RectTransform rectTransformCard;
    private RectTransform rectTransformSlot;

    private Transform parent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        parent = transform.parent;
        UnitCardSlot.OnAnyDropToSlot += UnitCardSlot_OnAnyDropToSlot;
    }

    private void Update()
    {
        if(!hasSlot && hasDropped)
        {
            transform.SetParent(parent, true);
            rectTransform.position = parent.GetComponent<RectTransform>().position;

            hasDropped = false;
        }

        if(hasSlot)
        {
            rectTransformCard.position = rectTransformSlot.position;

            hasSlot = false;
            hasDropped = false;
        }
    }

    private void UnitCardSlot_OnAnyDropToSlot(object sender, UnitCardSlot.OnAnyDropToSlotEventArgs e)
    {
        rectTransformSlot = e.rectTransformSlot;
        rectTransformCard = e.rectTransformCard;

        hasSlot = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.8f;
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(GameObject.Find("Canvas").transform, true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        hasDropped = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
}
