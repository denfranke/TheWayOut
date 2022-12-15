using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlaySystem : MonoBehaviour
{
    [SerializeField] private List<GameObject> overlays;
    [SerializeField] private Transform enemyPref;

    private void Start()
    {
        FindObjectOfType<DoorLock>().OpenDoor += DoorLock_ShowRoom;
    }

    private void DoorLock_ShowRoom()
    {
        foreach(GameObject overlay in overlays)
        {
            overlay.SetActive(false);
        }

        Instantiate(enemyPref, LevelGrid.Instance.GetWorldPosition(new GridPosition(4, 13)), Quaternion.Euler(0, 180, 0));
        Instantiate(enemyPref, LevelGrid.Instance.GetWorldPosition(new GridPosition(5, 13)), Quaternion.Euler(0, 180, 0));

        UnitManager.Instance.HiddenUnits -= 2;
    }
}
