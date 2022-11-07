using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpinAction : BaseAction
{
    private float totalSpeenAmount;

    void Update()
    {
        if (!isActive) return;

        float spinAmount = 360f * Time.deltaTime;
        totalSpeenAmount += spinAmount;
        transform.eulerAngles += new Vector3(0, spinAmount, 0);

        if (totalSpeenAmount >= 360)
        {
            isActive = false;
            OnActionComplete();
        }
    }
    public void Spin(Action OnActionComplete)
    {
        this.OnActionComplete = OnActionComplete;
        isActive = true;
        totalSpeenAmount = 0;
    }
}
