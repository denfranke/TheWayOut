using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScreenShakeActions : MonoBehaviour
{
    void Start()
    {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
    }

    private void ShootAction_OnAnyShoot(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake();
    }
}
