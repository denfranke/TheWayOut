using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    private GridObject gridObject;
    private TextMeshProUGUI textMeshPro;

    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }

    private void Update()
    {
        textMeshPro = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        textMeshPro.text = gridObject.ToString();
    }
}
