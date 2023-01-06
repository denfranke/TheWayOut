using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class DragDropCard : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private Transform parentAfterDrag;
    private Transform initialParent;

    private bool hasDropped = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        parentAfterDrag = transform.parent;
        initialParent = transform.parent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.8f;
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(transform.root, true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if(hasDropped)
            transform.SetParent(parentAfterDrag, true);
        else
            transform.SetParent(initialParent, true);

        hasDropped = false;
    }

    public Transform ParentAfterDrag { get { return parentAfterDrag; } set { parentAfterDrag = value; } }

    public Transform InitialParent { get { return initialParent; } }

    public bool HasDropped { get { return hasDropped; } set { hasDropped = value; } }
}
